using AnimationNode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AnimationNode
{
    public static class PathEllipse
    {

        public static DependencyObject[] GetAnimation(Point startPoint, Point endPoint, double diameter, PathGeometry geometry, double l, byte[] rgb, Storyboard m_Sb, double m_Speed, string m_PointData,FrameworkElement elm)
        {

            System.Windows.Controls.Grid grid = GetRunPoint(rgb, m_PointData,elm);

            Ellipse ell = PathEllipse.GetToEllipse(diameter, diameter, rgb, endPoint);

            double pointTime = l / m_Speed;

            StoryBoard.AddPointToStoryboard(grid, ell, m_Sb, geometry, l, startPoint, endPoint, pointTime);

            return new DependencyObject[] { grid, ell };
        }


        /// <summary>
        /// the body that moves along the connection
        /// </summary>
        /// <returns>Grid</returns>
        public static Grid GetRunPoint(byte[] rgb, string m_PointData,FrameworkElement ell=null)
        {

            //Grid
            Grid grid = new Grid
            {
                IsHitTestVisible = false,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 40,
                Height = 15,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };

            ell = ell??new Ellipse
            {
                Width = 40,
                Height = 15,
                Fill = new SolidColorBrush(Color.FromArgb(255, rgb[0], rgb[1], rgb[2])),
                OpacityMask = new RadialGradientBrush
                {
                    GradientOrigin = new Point(0.8, 0.5),
                    GradientStops = new GradientStopCollection(new[]
                {
                    new GradientStop(Color.FromArgb(255, 0, 0, 0), 0),
                    new GradientStop(Color.FromArgb(22, 0, 0, 0), 1)
                })
                }
            };

            //avoid Specified element is already the logical child of another element. Disconnect it first.
            var newell = ell.XamlClone();

            grid.Children.Add(newell);

            Path path = new Path
            {
                Data = Geometry.Parse(m_PointData),
                Width = 30,
                Height = 4,
                Fill = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 0),
                    GradientStops = new GradientStopCollection(
                        new[]{
                            new GradientStop(Color.FromArgb(88, rgb[0], rgb[1], rgb[2]), 0),
                            new GradientStop(Color.FromArgb(255, 255, 255, 255), 1)
                        })
                },
                Stretch = Stretch.Fill
            };

            //grid.Children.Add(ell);
            grid.Children.Add(path);

            return grid;
        }


        /// <summary>
        /// the ellipse at the target
        /// </summary>
        public static Ellipse GetToEllipse(double width, double height, byte[] rgb, Point toPos) => new Ellipse
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Width = width,
            Height = height,
            Fill = new SolidColorBrush(Color.FromArgb(255, rgb[0], rgb[1], rgb[2])),
            RenderTransform = new TranslateTransform(toPos.X - width / 2, toPos.Y - height / 2),
            Opacity = 0,

        };



        public static Path GetParticlePath(Point start, Point end, byte[] rgb, Storyboard sb, PathGeometry geometry, double particleTime)
        {

            Path path = new Path
            {
                Style = (Style)Application.Current.Resources["ParticlePathStyle"],
                Data = geometry,
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, rgb[0], rgb[1], rgb[2])),
                OpacityMask = StoryBoard.GetGradientBrush(start, end),

            };

            DoubleAnimation pda0 = StoryBoard.Animation3(particleTime);

            Storyboard.SetTarget(pda0, path);
            Storyboard.SetTargetProperty(pda0, new PropertyPath("(Path.OpacityMask).(GradientBrush.GradientStops)[0].(GradientStop.Offset)"));
            sb.Children.Add(pda0);


            var pda1 = StoryBoard.Animation3(particleTime);
            Storyboard.SetTarget(pda1, path);
            Storyboard.SetTargetProperty(pda1, new PropertyPath("(Path.OpacityMask).(GradientBrush.GradientStops)[1].(GradientStop.Offset)"));
            sb.Children.Add(pda1);


            return path;
        }

        //public static PathGeometry GetParticlePathGeometry(Point start, Point end, double angle)
        //{

        //    PathGeometry pathGeometry = new PathGeometry();
        //    PathFigure pf = new PathFigure();
        //    pf.StartPoint = start;
        //    ArcSegment arc = new ArcSegment();
        //    arc.SweepDirection = SweepDirection.Clockwise;
        //    arc.Point = end;

        //    double sinA = Math.Sin(Math.PI * angle / 180.0);

        //    double x = start.X - end.X;
        //    double y = start.Y - end.Y;
        //    double aa = x * x + y * y;
        //    double l = Math.Sqrt(aa);
        //    double r = l / (sinA * 2);
        //    arc.Size = new Size(r, r);
        //    pf.Segments.Add(arc);
        //    pathGeometry.Figures.Add(pf);

        //    return pathGeometry;
        //}
    }



}
