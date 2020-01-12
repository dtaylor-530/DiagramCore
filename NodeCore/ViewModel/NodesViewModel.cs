using GeometryCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NodeCore.ViewModel
{
    class NodesViewModel
    {
        private NodeViewModel point;
        private ObservableCollection<NodeViewModel> points;
        //private IEnumerable<ConnectionViewModel> _connections;

        public NodesViewModel()
        {
            var p = new NodeViewModel(250, 250);
            var ps = NodeFactory.SelectCircleCoordinates(250, 250, 200, 6).ToArray();
            //_connections = ConnectionFactory.Build(p, ps);
            //Observable.Interval(TimeSpan.FromSeconds(2)).Zip(ps.ToObservable().StartWith(p), (a, b) => b)
            //    .ObserveOn(App.Current.Dispatcher)
            //    .Subscribe(p =>
            //    {
            //        Points.Add(p);
            //    });
            
            //points = new ObservableCollection<INode>();

            points.CollectionChanged += Points_CollectionChanged;
        }

        private void Points_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
          //  var ps = NodeFactory.SelectCircleCoordinates(250, 250, 200, points.Count).ToArray();


        }

        public NodeViewModel Point { get { return point; } set { value.X = 250; value.Y = 250; } }

        //public ObservableCollection<INode> Points { get; } => points;

        //public ObservableCollection<ConnectionViewModel> Connections => new ObservableCollection<ConnectionViewModel>(_connections);

    }
}
