using System;
using System.Collections;
using System.Collections.Generic;
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


        public static readonly DependencyProperty IsDraggableProperty =            DependencyProperty.Register("IsDraggable", typeof(bool), typeof(NodesControl), new PropertyMetadata(true));


        public object SelectedObject
        {
            get { return (object)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }


        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(NodesControl), new PropertyMetadata(null));

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            foreach (var x in (IEnumerable)e.NewValue)
            {
                ((PointViewModel)x).PropertyChanged -= (a, b) => NodesControl_PropertyChanged(d, x, b.PropertyName);
                ((PointViewModel)x).PropertyChanged += (a, b) => NodesControl_PropertyChanged(d, x, b.PropertyName);

            }
        }

        private static void NodesControl_PropertyChanged(object sender, object e, string propertyName)
        {
            var nodesControl = (NodesControl)sender;
            var viewModel = ((PointViewModel)e);

            if (propertyName == nameof(PointViewModel.IsSelected))
            {
                foreach (var x in nodesControl.Items)
                {
                    if (viewModel != ((PointViewModel)x))
                        (x as PointViewModel).IsSelected = false;
                }

                nodesControl.Dispatcher.InvokeAsync(() => nodesControl.SelectedObject = viewModel, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
            }
            if (nodesControl.IsDraggable == false && (propertyName == nameof(PointViewModel.X) || propertyName == nameof(PointViewModel.Y)))
            {

                DoubleAnimation speedDoubleAni = null;
                var item = nodesControl.ItemContainerGenerator.ContainerFromItem(e);

                if (propertyName == nameof(PointViewModel.X))
                {
                    speedDoubleAni =
                            new DoubleAnimation(viewModel.OldX, (double)(viewModel.X), new Duration(new TimeSpan(0, 0, 1)));
                    Storyboard.SetTargetProperty(speedDoubleAni, new PropertyPath(Canvas.LeftProperty));
                }
                if (propertyName == nameof(PointViewModel.Y))
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
