using ConnectionCore;
using Munklesoft.Common.Collections;
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

namespace DiagramCore.DemoApp.ViewModel
{

    public class Design2Data
    {

        List<NodeViewModel> points1;
        List<NodeViewModel> points2;
        List<NodeViewModel> points3;
        ConnectionViewModel[] _connections;
        private GroupedObservableCollection<int, NodeViewModel> collection;

        public Design2Data()
        {

            points1 = new List<NodeViewModel>(new[]{
                new NodeViewModel(1) { GroupKey=1,  Object=new Rectangle { Fill=Brushes.Blue,  Height=10, Width=40 }},
                new NodeViewModel(2) {GroupKey=1,  Object=new Rectangle { Fill=Brushes.Red,   Height=10, Width=40 }},
                new NodeViewModel(3) {GroupKey=1,   Object=new Rectangle { Fill=Brushes.Green, Height=10, Width=40 }}
            });

            points2 = new List<NodeViewModel>(new[]{

                new NodeViewModel(5) {GroupKey=2,  Object=new Rectangle { Fill=Brushes.Red,   Height=10, Width=40 }},
                new NodeViewModel(6) {GroupKey=2,   Object=new Rectangle { Fill=Brushes.Green, Height=10, Width=40 }},
                new NodeViewModel(4) {GroupKey=2,   Object=new Rectangle { Fill=Brushes.Blue,  Height=10, Width=40 }},
            });


            points3 = new List<NodeViewModel>(new[]{

                new NodeViewModel(7) {GroupKey=3,  Object=new Rectangle { Fill=Brushes.Red,   Height=10, Width=40 }},
                new NodeViewModel(8) {GroupKey=3,   Object=new Rectangle { Fill=Brushes.Blue,  Height=10, Width=40 }},
                new NodeViewModel(9) {GroupKey=3,   Object=new Rectangle { Fill=Brushes.Green, Height=10, Width=40 }}
            });

            _connections = Connect(points1, points2).Concat(Connect(points2, points3)).ToArray();

            collection = new GroupedObservableCollection<int, NodeViewModel>(a => (int)a.GroupKey, points1.Concat(points2).Concat(points3));

        }



        private IEnumerable<ConnectionViewModel> Connect(IList<NodeViewModel> list1, IList<NodeViewModel> list2)
        {
            return from one in list1
                   join
                   two in list2
                   on ((one.Object as Rectangle).Fill as SolidColorBrush).Color equals ((two.Object as Rectangle).Fill as SolidColorBrush).Color
                   select new ConnectionViewModel(one, two);
        }

        public IEnumerable<Grouping<int, NodeViewModel>> Points => collection;


        //public List<NodeViewModel> Points2 => points2;

        public ConnectionViewModel[] Connections => _connections;





    }
}
