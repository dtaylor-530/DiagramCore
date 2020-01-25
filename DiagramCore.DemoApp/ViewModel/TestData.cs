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




    public class TestData : Jellyfish.ObservableObject
    {
        Random random = new Random();
        private int delay = 1000;
        ObservableCollection<NodeViewModel> points;

        Lazy<ObservableCollection<ConnectionViewModel>> _connections;
        private int yThreshold;

        public TestData()
        {

            points = new ObservableCollection<NodeViewModel>(new[]{
                new Node4ViewModel(50,  50,1) ,
                new Node4ViewModel( 500,  50,2),
                new Node4ViewModel (240, 250,3),
            });

            _connections = new Lazy<ObservableCollection<ConnectionViewModel>>(() =>
                  new ObservableCollection<ConnectionViewModel>{
                    new ConnectionViewModel(points[0],points[2]){Delay=delay },
                    new ConnectionViewModel(points[1],points[2]){ Delay=delay}
                     });

            this.Add = new Jellyfish.RelayCommand(a=>this.AddNode());
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

        public ObservableCollection<NodeViewModel> Points => points;

        public ObservableCollection<ConnectionViewModel> Connections => _connections.Value;



        public int Delay
        {
            get => delay;
            set { delay = value; this.Notify(nameof(Delay)); }
        }

        public int YThreshold
        {
            get => yThreshold;
            set { yThreshold = value; this.Notify(nameof(YThreshold)); }
        }


        public ICommand Add { get; }

   
        private void AddNode()
        {

            var node = new Node4ViewModel(350, 350, 4);

            foreach (var point in points)
            {
                _connections.Value.Add(new ConnectionViewModel(node, point, false) { Delay = delay });
            }

            points.Add(node);

        }
    }
}
