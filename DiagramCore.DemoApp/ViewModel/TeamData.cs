using ConnectionCore;
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
using System.Windows.Shapes;

namespace DiagramCore.DemoApp
{
    public class TeamData
    {
        Random random = new Random();
        private int delay = 1000;

        List<NodeViewModel> _points;
        Lazy<ConnectionViewModel[]> _connections;
        private int yThreshold;

        public TeamData()
        {
            _points = new List<NodeViewModel>(new[]{
                new Node5ViewModel(50,50,1) { Object=new Rectangle { Fill=Brushes.Blue,  Height=10, Width=40 }},
                new Node5ViewModel(550,50,2) {   Object=new Rectangle { Fill=Brushes.Red,   Height=10, Width=40 }},
                new Node5ViewModel(300,250,3) {  Object=new Rectangle { Fill=Brushes.Green, Height=10, Width=40 }},
                new Node5ViewModel(300,500,4) {  Object=new Rectangle { Fill=Brushes.Green, Height=10, Width=40 }}
            });

            _connections = new Lazy<ConnectionViewModel[]>(() =>

                  new[]{
                    new Connection2ViewModel(_points[0],_points[2], false){Delay=delay },
                    new Connection2ViewModel(_points[1],_points[2],true){ Delay=delay},
                    new Connection2ViewModel(_points[2],_points[3],true){ Delay=delay}
               });


            using (var values = Values().GetEnumerator())
            {
                Observable.Interval(TimeSpan.FromSeconds(2))
                  .ObserveOn(App.Current.Dispatcher)
                  .Subscribe(p =>
                  {
                      values.MoveNext();

                      _points[0].Y = (int)(values.Current.a * 10);
                      _points[1].Y = (int)(values.Current.b * 10);
                      _points[3].Y = (int)(values.Current.c * 10);
                  });
            }
        }

        public static IEnumerable<(double a, double b, double c)> Values()
        {
            Random random = new Random();

            while (true)
            {
                yield return (
                    MathNet.Numerics.Distributions.Normal.Sample(random, 3, 1),
                    MathNet.Numerics.Distributions.Normal.Sample(random, 2, 1),
                    MathNet.Numerics.Distributions.Normal.Sample(random, 1, 1));
            }
        }

        public List<NodeViewModel> Points => _points;

        public ConnectionViewModel[] Connections => _connections.Value;

    }
}
