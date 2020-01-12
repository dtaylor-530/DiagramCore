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

namespace DiagramCore.DemoApp
{




    public class DesignData : INotifyPropertyChanged
    {
        Random random = new Random();
        private int delay = 1000;
        NodeViewModel[] points;

        Lazy<ConnectionViewModel[]> _connections;

        public DesignData()
        {
            Move = new MoveCommand(this);

            points = new[]{
                new NodeViewModel { X = 50, Y = 50, Object=Brushes.Blue, Key=1},
                new NodeViewModel { X = 500, Y = 50, Object=Brushes.Red, Key=2},
                new NodeViewModel { X = 240, Y = 250, Object=Brushes.Green,Key=3}
            };

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
        }

        public NodeViewModel[] Points
        {
            get => points;
        }

        public ConnectionViewModel[] Connections => _connections.Value;


        public MoveCommand Move { get; }

        public int Delay
        {
            get => delay;
            set { delay = value; this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Delay))); }
        }




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

            foreach (var point in points)
            {
                point.X = random.Next(point.X - 30, point.X + 30);
                point.Y = random.Next(point.Y - 30, point.Y + 30);
            }

        }
    }
}
