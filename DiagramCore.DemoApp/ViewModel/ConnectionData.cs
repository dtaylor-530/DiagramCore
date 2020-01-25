
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




    public class ConnectionData
    {
        Random random = new Random();
        private int delay = 1000;
        ObservableCollection<INode> points = new ObservableCollection<INode>();
        Collection<INode> apoints = new Collection<INode>();

        ObservableCollection<ConnectionViewModel> connections = new ObservableCollection<ConnectionViewModel>();
        private int yThreshold;

        public ConnectionData()
        {

            var aNodeViewModel = new NodeViewModel(100, 150, "A") { CanChange = false };
            var bNodeViewModel = new NodeViewModel(300, 150, "B") { CanChange = false };

            points.Add(aNodeViewModel);
            points.Add(bNodeViewModel);
            var connection = new ConnectionViewModel(aNodeViewModel, bNodeViewModel);

            connections.Add(connection);

            using (var values = Values().GetEnumerator())
            {
                Observable.Interval(TimeSpan.FromSeconds(2))
                  .ObserveOn(App.Current.Dispatcher)
                  .Subscribe(p =>
                  {
                      values.MoveNext();

                      aNodeViewModel.Y = (int)(values.Current.x * 10);
                      bNodeViewModel.Y = (int)(values.Current.y * 10);
                  });
            }

        }


        public Collection<INode> Points => points;

        public ObservableCollection<ConnectionViewModel> Connections => connections;

        public static IEnumerable<(double x, double y)> Values()
        {
            Random random = new Random();
            const double factor = 3;

            while (true)
            {
                double val = random.Next(0, 20);
                yield return (val, MathNet.Numerics.Distributions.Normal.Sample(random, factor * val, 0.3));
            }
        }


        public static IEnumerable<INode> BuildCurve(int x, int y, int radius, int number)
        {
            return NodeFactory.
                SelectSineCoordinates(x, y, radius, number)
                .Select(
                (a, i) => new NodeViewModel((int)a.x, (int)a.y, i) { CanChange = false });
        }

    }
}

