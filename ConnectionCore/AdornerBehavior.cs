using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ConnectionCore
{
    public class AdornerBehavior : Behavior<UIElement>
    {

        EllipseAdorner adorner = null;



        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(AdornerBehavior), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseEnter += Element_MouseEnter;
            AssociatedObject.MouseLeave += Element_MouseLeave;

        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                AssociatedObject.MouseEnter -= Element_MouseEnter;
                AssociatedObject.MouseLeave -= Element_MouseLeave;
            }
        }

        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            AdornerLayer adLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);


            adorner = new EllipseAdorner(AssociatedObject, Command);
            adorner.MouseLeave += Ad_MouseLeave;

            adLayer.Add(adorner);

        }

        private void Ad_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(AssociatedObject) is AdornerLayer adornerLayer)
            {
                foreach (var adorner in AdornerLayer.GetAdornerLayer(AssociatedObject)?.GetAdorners(AssociatedObject).OfType<EllipseAdorner>().ToArray())
                {
                    DoubleAnimation doubleAnimation = new DoubleAnimation(0.0, new Duration(TimeSpan.FromSeconds(6)));
                    doubleAnimation.Completed += new EventHandler(fadeOutAnimation_Completed);
                    doubleAnimation.Freeze();

                    adorner.BeginAnimation(FrameworkElement.OpacityProperty, doubleAnimation);
                }
            }
        }

        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            if (adorner != null && adorner?.IsMouseOver == false)
            {
                Ad_MouseLeave(sender, e);
            }
        }

        private void fadeOutAnimation_Completed(object sender, EventArgs e)
        {
            if (adorner != null)
            {
                AdornerLayer adLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);
                if (adLayer != null)
                {
                    adorner.MouseLeave -= Ad_MouseLeave;
                    adLayer.Remove(adorner);
                }
            }
        }

        public class EllipseAdorner : Adorner
        {
            private ICommand command;

            public EllipseAdorner(UIElement adornedElement, ICommand command = null) : base(adornedElement)
            {
                this.MouseDown += EllipseAdorner_MouseDown;
                this.command = command;
            }

            private void EllipseAdorner_MouseDown(object sender, MouseButtonEventArgs e)
            {
                command?.Execute(this);
            }

            protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
            {

                SolidColorBrush renderBrush = new SolidColorBrush(Colors.LightCoral);

                Pen renderPen = new Pen(new SolidColorBrush(Colors.DarkBlue), 1.0);

         
                if (this.AdornedElement is Path path)
                {
                    double fraction = 0.5;  //the relative point of the curve
                    Point pt;               //the absolute point of the curve
                    Point tg;               //the tangent point of the curve
                    (path.RenderedGeometry as PathGeometry).GetPointAtFractionLength(
                        fraction,
                        out pt,
                        out tg);
                    drawingContext.DrawEllipse(renderBrush, renderPen, new Point(pt.X, pt.Y), 20, 20);
                }
                if (this.AdornedElement is PathPolyLine pathpolyline)
                {

                    var y = ((ConnectionPoint)pathpolyline.EndPoint).Position.Y + ((ConnectionPoint)pathpolyline.StartPoint).Position.Y;
                    var x = ((ConnectionPoint)pathpolyline.EndPoint).Position.X + ((ConnectionPoint)pathpolyline.StartPoint).Position.X;

                    drawingContext.DrawEllipse(renderBrush, renderPen, new Point(x/2, y/2), 20, 20);
                }
                else
                    drawingContext.DrawEllipse(renderBrush, renderPen, new Point(10, 10), 20, 20);
            }
        }
    }
}
