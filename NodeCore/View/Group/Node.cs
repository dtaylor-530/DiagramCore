using System.Windows;
using System.Windows.Controls;

namespace NodeCore.View
{
    public class Node : Control
    {

        static Node()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Node), new FrameworkPropertyMetadata(typeof(Node)));
        }

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(Node), new PropertyMetadata(60.0));


        public Node()
        {
        }

        public void RePosition(UIElement referenceElement)
        {
            //var xx = this.TransformToAncestor(dsf.TryFindParent<Grid>(this.VisualParent as StackPanel)).Transform(new Point(0, 0));
            var xy =
                this.TransformToAncestor(referenceElement ?? this.VisualParent as UIElement).Transform(new Point(0, 0));
            X = xy.X + this.ActualWidth / 2d - Size / 2d;
            Y = xy.Y + this.ActualHeight / 2d - Size / 2d;
        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Node), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Node), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
