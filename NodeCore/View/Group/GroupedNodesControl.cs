using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NodeCore.View
{

   
    public class GroupedNodesControl : ItemsControl
    {
        Subject<UIElement> changes = new Subject<UIElement>();

        static GroupedNodesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupedNodesControl), new FrameworkPropertyMetadata(typeof(GroupedNodesControl)));
        }

        public GroupedNodesControl()
        {
            //this.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            Observable.FromEventPattern(this.ItemContainerGenerator, nameof(ItemContainerGenerator.StatusChanged))
                .Where(a => this.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                .CombineLatest(
            Observable.FromEventPattern(this, nameof(this.Loaded))
                , changes, (a, b, c) => c)
                .Subscribe(ApplyReferenceElement);

        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void ApplyReferenceElement(UIElement referenceElement)
        {
            foreach (var item in this.Items.Cast<object>())
            {
                GetNodesControl(item).ReferenceElement = referenceElement;
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is KeyStackNodes;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var selectedContainer = new KeyStackNodes();
            return selectedContainer;
        }


        private KeyStackNodes GetNodesControl(object xx)
        {
            var myContentPresenter = ItemContainerGenerator.ContainerFromItem(xx);
            KeyStackNodes myDataTemplate = (myContentPresenter as KeyStackNodes);
            return myDataTemplate;
        }




        public UIElement ReferenceElement
        {
            get { return (UIElement)GetValue(ReferenceElementProperty); }
            set { SetValue(ReferenceElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReferenceElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReferenceElementProperty =
            DependencyProperty.Register("ReferenceElement", typeof(UIElement), typeof(GroupedNodesControl), new PropertyMetadata(null, Changed));


        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as GroupedNodesControl).changes.OnNext((UIElement)e.NewValue);
        }
    }
}
