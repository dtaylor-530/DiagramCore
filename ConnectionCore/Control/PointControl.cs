using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ConnectionCore
{
    public class PointControl : Control
    {
        static PointControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PointControl), new FrameworkPropertyMetadata(typeof(PointControl)));
        }

        public PointControl()
        {
        }


        private Path rctMovingObject;

        public override void OnApplyTemplate()
        {
            rctMovingObject = this.GetTemplateChild("PART_MovingObject") as Path;

            var myEllipseGeometry = Geometry == default(Geometry) ? GetGeometry() : Geometry;
            rctMovingObject.Data = myEllipseGeometry;
        }

        private static Geometry GetGeometry()
        {
            var myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = new Point(100, 50);
            myEllipseGeometry.RadiusX = 15;
            myEllipseGeometry.RadiusY = 15;
            return myEllipseGeometry;
        }

        public Geometry Geometry
        {
            get { return (Geometry)GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Geometry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GeometryProperty =
            DependencyProperty.Register("Geometry", typeof(Geometry), typeof(PointControl), new PropertyMetadata(default(EllipseGeometry)));




        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Delay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(int), typeof(PointControl), new PropertyMetadata(1000));


        public Point Point
        {
            get { return (Point)GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }

        public static readonly DependencyProperty PointProperty = DependencyProperty.Register("Point", typeof(Point), typeof(PointControl), new PropertyMetadata(default(Point), PointChanged));

        private static void PointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pc = d as PointControl;
            (pc.rctMovingObject?.Data as EllipseGeometry)?
                .BeginAnimation(EllipseGeometry.CenterProperty, new PointAnimation
                {
                    Duration = TimeSpan.FromMilliseconds(pc.Delay),
                    From = (Point)e.OldValue,
                    To = (Point)e.NewValue,
                    EasingFunction = new CubicEase()
                });
        }
    }

}
