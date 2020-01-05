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
        NodeViewModel[] points;

        Lazy<ConnectionViewModel[]> _connections;

        public DesignData()
        {
            Move = new MoveCommand(this);

            points = new[]{
            new NodeViewModel { X = 50, Y = 50, Object=1 },
                        new NodeViewModel { X = 500, Y = 50, Object=2},
                        new NodeViewModel { X = 240, Y = 250, Object=3 }
            };

         _connections = new Lazy<ConnectionViewModel[]>(() => 
                      new[]{
                    new ConnectionViewModel(points[0],points[2]),
                    new ConnectionViewModel(points[1],points[2])
                  });
        }




        public NodeViewModel[] Points
        {
            get => points;
        }

        public ConnectionViewModel[] Connections => _connections.Value;


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
