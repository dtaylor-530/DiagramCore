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

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            foreach (var x in (IEnumerable)(e.NewValue ?? Enumerable.Empty<object>()))
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

        private static void NodesControl_PropertyChanged(object sender, object e, string propertyName)
        {
            var nodesControl = (NodesControl)sender;
            var viewModel = ((NodeViewModel)e);

            if (propertyName == nameof(NodeViewModel.IsSelected))
            {
                foreach (var x in nodesControl.Items)
                {
                    if (viewModel != ((NodeViewModel)x))
                        (x as NodeViewModel).IsSelected = false;
                }

                nodesControl.Dispatcher.InvokeAsync(() => nodesControl.SelectedObject = viewModel, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
            }
            if (nodesControl.IsDraggable == false && (propertyName == nameof(NodeViewModel.X) || propertyName == nameof(NodeViewModel.Y)))
            {
                DoubleAnimation speedDoubleAni = null;
                var item = nodesControl.ItemContainerGenerator.ContainerFromItem(e);

                if (propertyName == nameof(NodeViewModel.X))
                {
                    speedDoubleAni =
                            new DoubleAnimation(viewModel.OldX, (double)(viewModel.X), new Duration(new TimeSpan(0, 0, 1)));
                    Storyboard.SetTargetProperty(speedDoubleAni, new PropertyPath(Canvas.LeftProperty));
                }
                if (propertyName == nameof(NodeViewModel.Y))
                {
                    speedDoubleAni =
                            new DoubleAnimation(viewModel.OldY, (double)(viewModel.Y), new Duration(new TimeSpan(0, 0, 1)));

                    Storyboard.SetTargetProperty(speedDoubleAni, new PropertyPath(Canvas.TopProperty));
                }

                Storyboard.SetTarget(speedDoubleAni, item);
                Storyboard story = new Storyboard();
                story.Children.Add(speedDoubleAni);
                story.Begin();
            }
        }

        private static object ItemsSourceCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

    }
}
