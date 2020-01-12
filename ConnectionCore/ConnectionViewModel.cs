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
        private int delay;

        private INode node1;
        private INode node2;
        private Point point;

        public ConnectionViewModel(INode node1, INode node2)
        {
            this.node1 = node1;
            this.node2 = node2;
            node1.PropertyChanged += PropertyChanged1;
            node2.PropertyChanged += PropertyChanged2;
        }



        private void PropertyChanged1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != string.Empty)
            {
                Point = new Point(X1, Y1);
                Point = new Point(X2, Y2);
            }
            if (e.PropertyName == nameof(node1.X))
                RaisePropertyChanged(nameof(this.X1));
            if (e.PropertyName == nameof(node1.Y))
                RaisePropertyChanged(nameof(this.Y1));
            Task.Delay(Delay).ContinueWith(a =>
            {
     
                if (e.PropertyName != string.Empty)
                {
                    var property = sender.GetType().GetProperty(e.PropertyName).GetValue(sender);
                    var message = new Message(node1.Key, node2.Key, e.PropertyName, property);
                    Messages.Add(message);
                    node2.NextChange(message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void PropertyChanged2(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName!=string.Empty)
            {
                Point = new Point(X2, Y2);
                Point = new Point(X1, Y1);
            }
            if (e.PropertyName == nameof(node1.X))
                RaisePropertyChanged(nameof(this.X2));
            if (e.PropertyName == nameof(node1.Y))
                RaisePropertyChanged(nameof(this.Y2));

            Task.Delay(Delay).ContinueWith(a =>
               {
                   if (e.PropertyName != string.Empty)
                   {
                       var property = sender.GetType().GetProperty(e.PropertyName).GetValue(sender);
                       var message = new Message(node2.Key, node1.Key, e.PropertyName, property);
                       Messages.Add(message);
                       node1.NextChange(message);
                   }
               }, TaskScheduler.FromCurrentSynchronizationContext());
        }






        public int Delay
        {
            get => delay;
            set { if (delay != value) { this.delay = value; RaisePropertyChanged(); } }
        }



        public double X1 { get => node1.X + node1.Size / DivideFactor; }

        public double Y1 { get => node1.Y + node1.Size / DivideFactor; }

        public double X2 { get => node2.X + node1.Size / DivideFactor; }

        public double Y2 { get => node2.Y + +node1.Size / DivideFactor; }

        public Point Point { get => point; set { if (value != point) { point = value; RaisePropertyChanged(); } } }


        public ObservableCollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();


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
