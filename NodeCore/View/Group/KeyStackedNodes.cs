using System;
using System.Windows;
using System.Windows.Controls;

namespace NodeCore.View
{
    public class KeyStackNodes : Control
    {
        static KeyStackNodes()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyStackNodes), new FrameworkPropertyMetadata(typeof(KeyStackNodes)));
        }

        public UIElement ReferenceElement
        {
            get { return (UIElement)GetValue(ReferenceElementProperty); }
            set { SetValue(ReferenceElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReferenceElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReferenceElementProperty =
            DependencyProperty.Register("ReferenceElement", typeof(UIElement), typeof(KeyStackNodes), new PropertyMetadata(null));


        public override void OnApplyTemplate()
        {
            var xx = this.GetTemplateChild("StackedNodesControl") as StackedNodesControl;
            xx.ReferenceElement = ReferenceElement;
        }
    }
}
