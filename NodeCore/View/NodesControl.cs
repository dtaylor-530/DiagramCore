using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace NodeCore
{
    public class NodesControl : ItemsControl
    {

        static NodesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodesControl), new FrameworkPropertyMetadata(typeof(NodesControl)));
            NodesControl.ItemsSourceProperty.OverrideMetadata(typeof(NodesControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, ItemsSourceChanged, ItemsSourceCoerce));

        }

        public NodesControl()
        {
            AddXCommand = new AddXCommand(this);
        }

        public bool IsDraggable
        {
            get { return (bool)GetValue(IsDraggableProperty); }
            set { SetValue(IsDraggableProperty, value); }
        }


        public static readonly DependencyProperty IsDraggableProperty = DependencyProperty.Register("IsDraggable", typeof(bool), typeof(NodesControl), new PropertyMetadata(true));


        public object SelectedObject
        {
            get { return (object)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }


        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(NodesControl), new PropertyMetadata(null));




        public ICommand AddXCommand
        {
            get { return (ICommand)GetValue(AddXCommandProperty); }
            set { SetValue(AddXCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddXCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddXCommandProperty =
            DependencyProperty.Register("AddXCommand", typeof(ICommand), typeof(NodesControl), new PropertyMetadata(null));
        private Stack<NodeViewModel> Removals = new Stack<NodeViewModel>();

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            foreach (var x in ((IEnumerable)(e.NewValue ?? Enumerable.Empty<object>())).OfType<NodeViewModel>())
            {
                ((NodeViewModel)x).PropertyChanged += (a, b) => NodesControl_PropertyChanged(d, x, b.PropertyName);
            }
            if (e.NewValue is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += (a, b) => NotifyCollectionChanged_CollectionChanged(d as NodesControl, b);
            }
        }

        private static void NotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var nodeViewModel in e.NewItems.Cast<NodeViewModel>())
            {
                nodeViewModel.PropertyChanged += (a, b) => NodesControl_PropertyChanged(sender, nodeViewModel, b.PropertyName);
            }

        }

        private static void NodesControl_PropertyChanged(object sender, NodeViewModel viewModel, string propertyName)
        {
            var nodesControl = (NodesControl)sender;

            if (propertyName == nameof(NodeViewModel.IsSelected) && viewModel.IsSelected)
            {
                Deselect(nodesControl, viewModel);
            }
            if (propertyName == nameof(NodeViewModel.X) || propertyName == nameof(NodeViewModel.Y))
            {
                if (nodesControl.IsDraggable == false)
                {

                    PositionChanged(nodesControl, viewModel, propertyName);

                }
                nodesControl.Story_Completed(nodesControl);
                var match = nodesControl.GetMatchByX(viewModel);
                if (match != default && propertyName == nameof(NodeViewModel.X))
                {
                    nodesControl.Merge(match, viewModel);
                }

            }
        }

        private void Merge(NodeViewModel match, NodeViewModel viewModel)
        {

            var y = (int)((1d * match.Y * match.Size + viewModel.Y * viewModel.Size) / (match.Size + viewModel.Size));
            var size = (int)((match.Size + viewModel.Size) / 2d);


            match.Size = size;
            viewModel.Size = size;

            match.Y = y;
            Removals.Push(match);
            viewModel.Y = y;

        }

        private static object ItemsSourceCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }


        private static void Deselect(NodesControl nodesControl, NodeViewModel viewModel)
        {
            foreach (var item in nodesControl.Items)
            {
                if (viewModel != ((NodeViewModel)item))
                {
                    (item as NodeViewModel).IsSelected = false;
                }
            }

            nodesControl.Dispatcher.InvokeAsync(() => nodesControl.SelectedObject = viewModel,
                System.Windows.Threading.DispatcherPriority.Background, default);
        }

        private static Task<EventArgs> PositionChanged(NodesControl nodesControl, NodeViewModel viewModel, string propertyName, Action action = null)
        {
            DoubleAnimation speedDoubleAni = null;
            DependencyObject dObject = nodesControl.ItemContainerGenerator.ContainerFromItem(viewModel);

            if (propertyName == nameof(NodeViewModel.X))
            {
                speedDoubleAni =
                        new DoubleAnimation(viewModel.OldX, (double)(viewModel.X), new Duration(new TimeSpan(0, 0, 3)));
                Storyboard.SetTargetProperty(speedDoubleAni, new PropertyPath(Canvas.LeftProperty));
            }
            if (propertyName == nameof(NodeViewModel.Y))
            {
                speedDoubleAni =
                        new DoubleAnimation(viewModel.OldY, (double)(viewModel.Y), new Duration(new TimeSpan(0, 0, 3)));

                Storyboard.SetTargetProperty(speedDoubleAni, new PropertyPath(Canvas.TopProperty));
            }

            Storyboard.SetTarget(speedDoubleAni, dObject);
            Storyboard story = new Storyboard();
            story.Children.Add(speedDoubleAni);


            story.Completed += (a, b) => nodesControl.Story_Completed(nodesControl);

            story.Begin();

            return null;
        }

        private void Story_Completed(NodesControl nodesControl)
        {
            bool removed = false;
            while (Removals.TryPop(out NodeViewModel pop))
            {
                removed |= (nodesControl.ItemsSource as ICollection<NodeViewModel>).Remove(pop);
            }

            if (removed)
            {
                var list = nodesControl.ItemsSource;
                nodesControl.ItemsSource = null;
                nodesControl.ItemsSource = list;
            }
        }

        private NodeViewModel GetMatchByX(NodeViewModel nodeViewModel)
        {
            var item = this.Items.OfType<NodeViewModel>().Where(a => a.X == nodeViewModel.X && a.Y != nodeViewModel.Y).SingleOrDefault();

            return item;
        }


    }

    internal class AddXCommand : ICommand
    {
        private NodesControl nodesControl;

        const double factor = 0.1;

        public AddXCommand(NodesControl nodesControl)
        {
            this.nodesControl = nodesControl;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var x = Convert.ToInt32(parameter);
            var items = nodesControl.Items.OfType<NodeViewModel>().ToArray();

            var y = items.Sum(a => (a.Size - factor * Math.Abs(a.X - x)) * a.Y)
                 / items.Sum(a => (a.Size - factor * Math.Abs(a.X - x)));

            var size = (int)items.Average(a => a.Size - factor * Math.Abs(a.X - x));

            (nodesControl.ItemsSource as ICollection<NodeViewModel>).Add(new NodeViewModel(x, Convert.ToInt32(y), size));
            var list = nodesControl.ItemsSource as ICollection<NodeViewModel>;
            nodesControl.ItemsSource = null;
            nodesControl.ItemsSource = list;
        }
    }

}