using ConnectionCore;
using NodeCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DiagramCore.DemoApp
{




    public class DesignData
    {
        public DesignData()
        {
            Move = new MoveCommand(this);
        }

        PointViewModel[] points = 
            new[]{
            new PointViewModel { X = 50, Y = 50 },
                        new PointViewModel { X = 500, Y = 50 },
                        new PointViewModel { X = 240, Y = 250 }
            };

        public ObservableCollection<PointViewModel> Points
        {
            get => new ObservableCollection<PointViewModel>( points);
        }
        public ObservableCollection<ConnectionViewModel> Connections
        {
            get
            {
                var _connections = new Lazy<ObservableCollection<ConnectionViewModel>>(() => new ObservableCollection<ConnectionViewModel>(
                    new[]{
                    new ConnectionViewModel(points[0],points[2]),
                    new ConnectionViewModel(points[1],points[2])
                })); 
                return _connections.Value;
            }
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

        Random random = new Random();

        private void MoveNodes()
        {
           
            foreach(var point in points)
            {
                point.X = random.Next(point.X - 30, point.X + 30);
                point.Y = random.Next(point.Y - 30, point.Y + 30);
            }

        }
    }
}
