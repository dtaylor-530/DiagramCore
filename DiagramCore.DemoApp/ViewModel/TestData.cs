using ConnectionCore;
using GeometryCore;
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




    public class TestData : INotifyPropertyChanged
    {
        Random random = new Random();
        private int delay = 1000;
        ObservableCollection<NodeViewModel> points;

        Lazy<ObservableCollection<ConnectionViewModel>> _connections;
        private int yThreshold;

        public TestData()
        {

            points = new ObservableCollection<NodeViewModel>(new[]{
                new Node4ViewModel(50,  50,1) { CanChange=false},
                new Node4ViewModel( 500,  50,2){   CanChange=false},
                new Node4ViewModel (  240,  250,3) { CanChange = false },
            });

            _connections = new Lazy<ObservableCollection<ConnectionViewModel>>(() =>
                  new ObservableCollection<ConnectionViewModel>{
                    new ConnectionViewModel(points[0],points[2]){Delay=delay },
                    new ConnectionViewModel(points[1],points[2]){ Delay=delay}
                     });

            this.Add = new AddCommand(this);
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
            private TestData pvm;

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }
            public AddCommand(TestData pvm)
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
                _connections.Value.Add(new ConnectionViewModel(node, point, false) { Delay = delay });
            }

            points.Add(node);

        }
    }


    public class Node4ViewModel : NodeViewModel
    {

        public Node4ViewModel(int x, int y, object key) : base(x, y, key)
        {
            CanChange = false;

        }
        public override void NextMessage(IMessage message)
        {
            if (message.Key.ToString() == nameof(NodeViewModel.Y))
            {
                (int val, double weight) = ((int, double))message.Content;


                //var node = Nodes.SingleOrDefault(a => a.Key.Equals(message.From));

                //if (node != null)
                //{
                var content = (val * weight + this.Y * this.Size) / (weight + this.Size);
                //message = new Message(message.From, message.To, message.Key, content);
                //base.NextMessage(message);
                this.Y = (int) content;
                //}
            }
            
            else
            {
                base.NextMessage(message);
            }
            
        }
    }
}
