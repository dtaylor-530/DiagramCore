using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NodeCore.View
{


    public class StackedNodesControl :ItemsControl
    {
        public StackedNodesControl()
        {
            this.Loaded += StackedNodesControl_Loaded;
        }

        public UIElement ReferenceElement
        {
            get { return (UIElement)GetValue(ReferenceElementProperty); }
            set { SetValue(ReferenceElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReferenceElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReferenceElementProperty =
            DependencyProperty.Register("ReferenceElement", typeof(UIElement), typeof(StackedNodesControl), new PropertyMetadata(null,Changed));

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nc = d as StackedNodesControl;
            nc.ReferenceElement = e.NewValue as UIElement;
            nc.RepositionNodes(nc.ReferenceElement);
        }

        private void StackedNodesControl_Loaded(object sender, RoutedEventArgs e)
        {
            RepositionNodes(ReferenceElement ?? (this.VisualParent as UIElement));
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is Node;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var selectedContainer = new Node();
            return selectedContainer;
        }


        protected override Size ArrangeOverride(Size availableSize)
        {
            var size = base.ArrangeOverride(availableSize);
            RepositionNodes(ReferenceElement);
            return size;
        }

        private void RepositionNodes(UIElement element)
        {
            foreach (var item in Items.Cast<object>().Select(a => this.ItemContainerGenerator.ContainerFromItem(a)).OfType<Node>())
            {
                item.RePosition(element);
            }
        }
    }
}
