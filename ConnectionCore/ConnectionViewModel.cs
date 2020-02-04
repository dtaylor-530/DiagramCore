using ConnectionCore.Common;
using GeometryCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConnectionCore
{
    public class ConnectionViewModel : INotifyPropertyChanged
    {
        private const double DivideFactor = 2d;
        private static readonly string[] props = new[] { nameof(INode.X), nameof(INode.Y), nameof(INode.Size) };

        private int delay = 2000;
        private Point point1;
        private Point point2;
        private bool biDirectional = true;
        private bool isSelected;
        private ConnectionNodeViewModel node = new ConnectionNodeViewModel();

        protected double decayFactor = 0.02d;
        protected double XDistance => Math.Abs(X1 - X2);
        protected INode node1;
        protected INode node2;

        public ConnectionViewModel(INode node1, INode node2, bool birectional = true)
        {
            this.node1 = node1;
            this.node2 = node2;

            ObservableCollectionHelper.MakeObservable(node1.Messages).Subscribe(message =>
            {
                SendMessage1(message);
            });

            this.biDirectional = birectional;


            if (birectional)
            {
                ObservableCollectionHelper.MakeObservable(node2.Messages).Subscribe(message =>
                {
                    SendMessage2(message);
                });

            }

            var dis1 = ObservableCollectionHelper.MakeObservable(this.Messages1)
                               .Where(a => a.Key.Equals(nameof(INode.Y)))
                .Subscribe(a =>
                {
                    (int val, double deviation) = ((int, double))a.Content;
                    node.OnNext1(val);
                });

            var dis2 = ObservableCollectionHelper.MakeObservable(this.Messages2)
                .Where(a => a.Key.Equals(nameof(INode.Y)))
                .Subscribe(a =>
            {
                (int val, double deviation) = ((int, double))a.Content;
                node.OnNext2(val);
            });

            SelectCommand = new SelectCommand(this);
        }




        private void SendMessage1(IMessage message)
        {
            if (message.Key.Equals(string.Empty) == false &&
                props.Contains(message.Key.ToString()))
            {
                message = new Message(message.From, node2.Key, message.Key, message.Content);

                if (message.Key.Equals(nameof(node1.X)))
                    RaisePropertyChanged(nameof(this.X1));

                if (message.Key.Equals(nameof(node1.Y)))
                {
                    RaisePropertyChanged(nameof(this.Y1));
                    message = Modify(message, node1.Size);
                }

                MovePoint1To2();

                Task.Delay(Delay).ContinueWith(a =>
                {
                    Messages1.Add(message);
                    node2.NextMessage(message);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }


        private void SendMessage2(IMessage message)
        {

            if (message.Key.Equals(string.Empty) == false &&
                props.Contains(message.Key.ToString()) &&
                biDirectional == true)
            {
                message = new Message(message.From, node1.Key, message.Key, message.Content);

                if (message.Key.Equals(nameof(node1.X)))
                    RaisePropertyChanged(nameof(this.X2));

                if (message.Key.Equals(nameof(node1.Y)))
                {
                    RaisePropertyChanged(nameof(this.Y2));
                    message = Modify(message, node2.Size);
                }

                MovePoint2To1();

                Task.Delay(Delay).ContinueWith(a =>
                {
                    Messages2.Add(message);
                    node1.NextMessage(message);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }


        public int Delay
        {
            get => delay;
            set { if (delay != value) { this.delay = value; RaisePropertyChanged(); } }
        }

        public double X1 => node1.X + node1.Size / DivideFactor;

        public double Y1 => node1.Y + node1.Size / DivideFactor;

        public double X2 => node2.X + node1.Size / DivideFactor;

        public double Y2 => node2.Y + +node1.Size / DivideFactor;

        public Point Point1 { get => point1; set { if (value != point1) { point1 = value; RaisePropertyChanged(); } } }

        public Point Point2 { get => point2; set { if (value != point2) { point2 = value; RaisePropertyChanged(); } } }


        public double DecayFactor { get => decayFactor; set { if (value != decayFactor) { decayFactor = value; RaisePropertyChanged(); } } }

        public bool BiDirectional { get => biDirectional; set { if (value != biDirectional) { biDirectional = value; RaisePropertyChanged(); } } }

        public ObservableCollection<IMessage> Messages1 { get; } = new ObservableCollection<IMessage>();

        public ObservableCollection<IMessage> Messages2 { get; } = new ObservableCollection<IMessage>();

        public ConnectionNodeViewModel Node => node;

   

        #region propertychanged

        [Browsable(false)]
        public event PropertyChangedEventHandler PropertyChanged;
        //public event Action<IMessage> MessageChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value != isSelected) { isSelected = value; RaisePropertyChanged(); };
            }
        }


        [Browsable(false)]
        public ICommand SelectCommand { get; }


        protected virtual IMessage Modify(IMessage message, int size)
        {
            if (message.Key.Equals(nameof(node1.Y)))
            {
                var val = System.Convert.ToInt32(message.Content);

                // the decay (like half-life)
                var weight = size * Math.Exp(-decayFactor * XDistance);

                message = new Message(message.From, message.To, message.Key, (val, weight));

            }
            return message;
        }

        private void MovePoint1To2() => MovePoint1(new Point(X1, Y1), new Point(X2, Y2));

        private void MovePoint2To1() => MovePoint2(new Point(X2, Y2), new Point(X1, Y1));

        private void MovePoint1(Point one, Point two)
        {
            Point1 = one;
            Point1 = two;

        }

        private void MovePoint2(Point one, Point two)
        {
            Point2 = one;
            Point2 = two;

        }

    }


    public class SelectCommand : ICommand
    {
        private ConnectionViewModel pvm;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public SelectCommand(ConnectionViewModel pvm)
        {
            this.pvm = pvm;
        }
        public void Execute(object parameter)
        {
            if (pvm != null)
                (pvm).IsSelected = true;
        }
    }
}
