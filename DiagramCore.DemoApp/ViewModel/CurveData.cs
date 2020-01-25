
using ConnectionCore;
using GeometryCore;
using NodeCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace DiagramCore.DemoApp
{




    public class CurveData : INotifyPropertyChanged
    {
        Random random = new Random();
        private int delay = 1000;
        ObservableCollection<INode> points = new ObservableCollection<INode>();
        Collection<INode> apoints = new Collection<INode>();

        ObservableCollection<ConnectionViewModel> connections = new ObservableCollection<ConnectionViewModel>();
        private int yThreshold;

        public CurveData()
        {



            var ps = BuildCurve(250, 250, 200, 6).ToArray();

            this.Add = new AddCommand(this);
            this.PropertyChanged += DesignData_PropertyChanged;
 
            Observable.Interval(TimeSpan.FromSeconds(2)).Zip(ps.ToObservable(), (a, b) => b)
              .ObserveOn(App.Current.Dispatcher)
              .Subscribe(p =>
              {
               
                  var apoint = new Node4ViewModel(p.X, 100, "A") { Size = 30 };
                  apoints.Add(apoint);
                  points.Add(apoint);
                  foreach (var _apoint in apoints)
                  {
                      var conn = new ConnectionViewModel(p, _apoint, false) { DecayFactor = 0.2d };
                      connections.Add(conn);
                  }
                  points.Add(p);

              });

        }

        public static IEnumerable<INode> BuildCurve(int x, int y, int radius, int number)
        {
            return NodeFactory.
                SelectSineCoordinates(x, y, radius, number)
                .Select(
                (a, i) => new NodeViewModel((int)a.x, (int)a.y, i) { CanChange = false });
        }



        private void DesignData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Delay))
            {
                foreach (var connection in connections)
                {
                    connection.Delay = Delay;
                }
            }
            //if (e.PropertyName == nameof(YThreshold))
            //{
            //    foreach (var node in points)
            //    {
            //        node.YThreshold = YThreshold;
            //    }
            //}
        }

        public ObservableCollection<INode> Points => points;

        public ObservableCollection<ConnectionViewModel> Connections => connections;

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




        public event PropertyChangedEventHandler PropertyChanged;



        public AddCommand Add { get; }

        public class AddCommand : ICommand
        {
            private CurveData pvm;

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }
            public AddCommand(CurveData pvm)
            {
                this.pvm = pvm;
            }


            public void Execute(object parameter)
            {
                (pvm)?.AddNode();
            }
        }

        private void AddNode()
        {

            var node = new Node4ViewModel(350, 350, 4);

            foreach (var point in points)
            {
                connections.Add(new ConnectionViewModel(node, point, false) { Delay = delay });
            }

            points.Add(node);

        }
    }
}

