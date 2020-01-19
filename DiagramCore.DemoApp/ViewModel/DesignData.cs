using ConnectionCore;
using NodeCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiagramCore.DemoApp
{




    public class DesignData : INotifyPropertyChanged
    {
        Random random = new Random();
        private int delay = 1000;
        List<NodeViewModel> points;

        Lazy<ConnectionViewModel[]> _connections;
        private int yThreshold;

        public DesignData()
        {
            Move = new MoveCommand(this);

            points = new List<NodeViewModel>(new[]{ 
                new NodeViewModel(1) { X = 50,  Y = 50,  Object=new Rectangle { Fill=Brushes.Blue,  Height=10, Width=40 }},
                new NodeViewModel(2) { X = 500, Y = 50,  Object=new Rectangle { Fill=Brushes.Red,   Height=10, Width=40 }},
                new NodeViewModel(3) { X = 240, Y = 250, Object=new Rectangle { Fill=Brushes.Green, Height=10, Width=40 }}
            });

            _connections = new Lazy<ConnectionViewModel[]>(() =>
                  new[]{
                    new ConnectionViewModel(points[0],points[2]){Delay=delay },
                    new ConnectionViewModel(points[1],points[2]){ Delay=delay}
                     });


            this.PropertyChanged += DesignData_PropertyChanged;
        }

        private void DesignData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Delay))
            {
                foreach (var connection in _connections.Value)
                {
                    connection.Delay = Delay;
                }
            }
            if (e.PropertyName == nameof(YThreshold))
            {
                foreach (var node in points)
                {
                    node.YThreshold = YThreshold;
                }
            }
        }

        public List<NodeViewModel> Points => points;

        public ConnectionViewModel[] Connections => _connections.Value;



        public int Delay
        {
            get => delay;
            set { delay = value; this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Delay))); }
        }

        public int YThreshold
        {
            get => yThreshold;
            set { yThreshold = value; this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(YThreshold))); }
        }


        public MoveCommand Move { get; }

        public class MoveCommand : ICommand
        {
            private DesignData pvm;

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }
            public MoveCommand(DesignData pvm)
            {
                this.pvm = pvm;
            }


            public void Execute(object parameter)
            {
                (pvm)?.MoveNodes();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void MoveNodes()
        {

            foreach (var point in points.ToArray())
            {
                point.X = random.Next(point.X - 30, point.X + 30);
                point.Y = random.Next(point.Y - 30, point.Y + 30);
            }

        }
    }
}
