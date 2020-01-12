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
using System.Windows.Threading;

namespace DiagramCore.DemoApp
{




    public class DesignData1
    {
        private IEnumerable<ConnectionViewModel> _connections;

        public DesignData1()
        {
            var p = new NodeViewModel(250, 250);
            var ps = BuildCircle(250, 250, 200, 6).ToArray();
            _connections = ConnectionFactory.Build(p, 1000, ps);
            Observable.Interval(TimeSpan.FromSeconds(2)).Zip(ps.ToObservable().StartWith(p), (a, b) => b)
                .ObserveOn(App.Current.Dispatcher)
                .Subscribe(p =>
            {
                Points.Add(p);
            });
        }


        public ObservableCollection<INode> Points { get; } = new ObservableCollection<INode>();

        public ObservableCollection<ConnectionViewModel> Connections => new ObservableCollection<ConnectionViewModel>(_connections);

        public static IEnumerable<INode> BuildCircle(int x, int y, int radius, int number)
        {
            return NodeFactory.
                SelectCircleCoordinates(x, y, radius, number)
                .Select(a => new Node2ViewModel((int)a.x, (int)a.y));
        }



    }

    public class Node2ViewModel : NodeViewModel
    {

        public Node2ViewModel(int x, int y) : base(x, y)
        {
            CanChange = false;

        }
        public override void NextChange(IMessage message)
        {
            if (message.Key.ToString() == nameof(NodeViewModel.Size))
            {
                var val = (int)message.Content;
                this.Size = (int)(((int)val) / 2d);
            }
            base.NextChange(message);
        }
    }
}
