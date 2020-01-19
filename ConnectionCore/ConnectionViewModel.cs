using GeometryCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConnectionCore
{
    public class ConnectionViewModel : INotifyPropertyChanged
    {
        private const double DivideFactor = 2d;
        private int delay = 2000;
        private static readonly string[] props = new[] { nameof(INode.X), nameof(INode.Y), nameof(INode.Size) };
        private INode node1;
        private INode node2;
        private Point point;
        private bool biDirectional = true;
        private double decayFactor = 0.02d;

        public ConnectionViewModel(INode node1, INode node2, bool birectional = true)
        {
            this.node1 = node1;
            this.node2 = node2;

            foreach (var message in node1.Messages)
            {
                SendMessage1(message);
            }

            node1.PropertyChanged += PropertyChanged1;

            this.biDirectional = birectional;

            if (birectional)
                foreach (var message in node2.Messages)
                {
                    SendMessage2(message);
                }
            node2.PropertyChanged += PropertyChanged2;
        }



        private void PropertyChanged1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName != string.Empty &&
                props.Contains(e.PropertyName))
            {
                if (e.PropertyName == nameof(node1.X))
                    RaisePropertyChanged(nameof(this.X1));
                if (e.PropertyName == nameof(node1.Y))
                    RaisePropertyChanged(nameof(this.Y1));
       
                

                Point = new Point(X1, Y1);
                Point = new Point(X2, Y2);

                Task.Delay(Delay).ContinueWith(a =>
                {
                    var property = sender.GetType().GetProperty(e.PropertyName).GetValue(sender);
                    IMessage message = new Message(node1.Key, node2.Key, e.PropertyName, property);
                    if (message.Key.Equals(nameof(node1.Y)))
                        message = Modify(message, node1.Size);
                    Messages.Add(message);
                    node2.NextMessage(message);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
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
                //if (message.Key.Equals(nameof(node1.Size)))
                //    message = Modify(message, node1.Size);
                Point = new Point(X1, Y1);
                Point = new Point(X2, Y2);

                Task.Delay(Delay).ContinueWith(a =>
                {
                    Messages.Add(message);
                    node2.NextMessage(message);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }




        private void PropertyChanged2(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (biDirectional == true &&
                e.PropertyName != string.Empty &&
               props.Contains(e.PropertyName))
            {
                if (e.PropertyName == nameof(node1.X))
                    RaisePropertyChanged(nameof(this.X2));
                if (e.PropertyName == nameof(node1.Y))
                    RaisePropertyChanged(nameof(this.Y2));

                Point = new Point(X2, Y2);
                Point = new Point(X1, Y1);

                Task.Delay(Delay).ContinueWith(a =>
                   {
                       var property = sender.GetType().GetProperty(e.PropertyName).GetValue(sender);
                       IMessage message = new Message(node2.Key, node1.Key, e.PropertyName, property);
                       if (message.Key.Equals(nameof(node2.Y)))
                           message = Modify(message, node2.Size);
                       Messages.Add(message);
                       node1.NextMessage(message);
                   }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void SendMessage2(IMessage message)
        {

            if (biDirectional == true &&
                message.Key.Equals(string.Empty) == false &&
               props.Contains(message.Key.ToString()))
            {
                message = new Message(message.From, node1.Key, message.Key, message.Content);
                if (message.Key.Equals(nameof(node1.X)))
                    RaisePropertyChanged(nameof(this.X2));
                if (message.Key.Equals(nameof(node1.Y)))
                {
                    RaisePropertyChanged(nameof(this.Y2));
                    message = Modify(message, node2.Size);
                }


                Point = new Point(X2, Y2);
                Point = new Point(X1, Y1);

                Task.Delay(Delay).ContinueWith(a =>
                {
                    Messages.Add(message);
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

        public Point Point { get => point; set { if (value != point) { point = value; RaisePropertyChanged(); } } }

        //
        public double DecayFactor { get => decayFactor; set { if (value != decayFactor) { decayFactor = value; RaisePropertyChanged(); } } }

        public bool BiDirectional { get => biDirectional; set { if (value != biDirectional) { biDirectional = value; RaisePropertyChanged(); } } }

        public ObservableCollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();

        private IMessage Modify(IMessage message, int size)
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

        private double XDistance => Math.Abs(X1 - X2);

        #region propertychanged

        [Browsable(false)]
        public event PropertyChangedEventHandler PropertyChanged;
        //public event Action<IMessage> MessageChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
