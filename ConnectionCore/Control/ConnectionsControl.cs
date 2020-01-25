using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ConnectionCore
{
    public class ConnectionsControl:ItemsControl
    {

        public object SelectedObject
        {
            get { return (object)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }


        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(ConnectionsControl), new PropertyMetadata(null));



        static ConnectionsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConnectionsControl), new FrameworkPropertyMetadata(typeof(ConnectionsControl)));
            ConnectionsControl.ItemsSourceProperty.OverrideMetadata(typeof(ConnectionsControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, ItemsSourceChanged));
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            foreach (var x in ((IEnumerable)(e.NewValue ?? Enumerable.Empty<object>())).OfType<ConnectionViewModel>())
            {
                ((ConnectionViewModel)x).PropertyChanged += (a, b) => NodesControl_PropertyChanged(d, x, b.PropertyName);
            }
            if (e.NewValue is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += (a, b) => NotifyCollectionChanged_CollectionChanged(d as ConnectionsControl, b);
            }
        }

        private static void NotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var nodeViewModel in e.NewItems.Cast<ConnectionViewModel>())
            {
                nodeViewModel.PropertyChanged += (a, b) => NodesControl_PropertyChanged(sender, nodeViewModel, b.PropertyName);
            }

        }

        private static void NodesControl_PropertyChanged(object sender, ConnectionViewModel viewModel, string propertyName)
        {
            var nodesControl = (ConnectionsControl)sender;

            if (propertyName == nameof(ConnectionViewModel.IsSelected) && viewModel.IsSelected)
            {
                Reselect(nodesControl, viewModel);
            }
            
        }
        private static void Reselect(ConnectionsControl connectionsControl, ConnectionViewModel viewModel)
        {
            foreach (var item in connectionsControl.Items)
            {
                if (viewModel != ((ConnectionViewModel)item))
                {
                    (item as ConnectionViewModel).IsSelected = false;
                }
            }

            connectionsControl.Dispatcher.InvokeAsync(() => connectionsControl.SelectedObject = viewModel,
                System.Windows.Threading.DispatcherPriority.Background, default);
        }
    }
}
