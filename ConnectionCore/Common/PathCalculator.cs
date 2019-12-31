

using GeometryCore;

using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ConnectionCore
{

    public static class ConversionHelper
    {
        public static Point2D ToStartPoint2D(this Line line)
            => new Point2D(line.X1, line.Y1);
        public static Point2D ToEndPoint2D(this Line line)
     => new Point2D(line.X2, line.Y2);

        public static Point ToStartPoint(this Line line)
    => new Point(line.X1, line.Y1);
        public static Point ToEndPoint(this Line line)
     => new Point(line.X2, line.Y2);

        public static Point ToPoint(this Point2D point)
=> new Point(point.X, point.Y);

        public static Point2D ToPoint2D(this Point point)
=> new Point2D(point.X, point.Y);


        public static Line2D ToLine2D(this Line line)
    => new Line2D(line.ToStartPoint2D(), line.ToEndPoint2D());
    }


    public enum PathLine { Straight = 1, Orthogonal = 2 }

    public static class PathCalculator
    {

        public static List<Point2D> FindPath(Line lineA, Line lineB, PathLine pathLine = PathLine.Orthogonal, double angle = 0)
        {
            List<Point2D> linkPoints = new List<Point2D>();
            linkPoints.Add(lineA.ToStartPoint2D());

            switch (pathLine)
            {
                case PathLine.Straight:
                    break;
                case PathLine.Orthogonal:
                    GeometryCore.PathCalculator.DetermineOrthogonalPoints(lineA.ToLine2D(), lineB.ToLine2D(), ref linkPoints);
                    break;
            }
  
            linkPoints.Add(lineB.ToStartPoint2D());
            if (angle != 0)
            {
                List<Point> bpoints = null;
                switch (pathLine)
                {
                    case PathLine.Straight:
                       double radius = lineA.ToStartPoint2D().DetermineArcRadius(lineB.ToStartPoint2D(), angle);
                        bpoints=GetArcGeometry(lineA.ToStartPoint(), lineB.ToStartPoint(), radius).ToPointsList();
                        break;
                    case PathLine.Orthogonal:
                        bpoints = GetBezierGeometry(linkPoints.Select(a=>a.ToPoint()).ToList()).ToPointsList(); 
                        break;
                }
                linkPoints.Clear();
                foreach (var point in bpoints)
                    linkPoints.Add(point.ToPoint2D());
            }
            return linkPoints;
        }



        public static List<Point> ToPointsList(this PathGeometry geometry)
        {
            List<Point> outpoints = new List<Point>();
            for (int i = 1; i < 101; i++)
            {
                geometry.GetPointAtFractionLength(((double)i) / 100, out Point xd, out Point t);
                outpoints.Add(xd);
            }
            return outpoints;
        }

        //CaliDiagramAndRaft
        public static PathGeometry GetBezierGeometry(List<Point> points)
        {

            var myPathFigure = new PathFigure { StartPoint = points.FirstOrDefault() };
            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();

            BezierSegment segment = null;

            if(points.Count==3)
 
                segment = new BezierSegment
                {
                    Point1 = points[1],
                    Point2 = points[1],
                    Point3 = points[2],
                };
            else if (points.Count == 4)
            {
                segment = new BezierSegment
                {
                    Point1 = points[1],
                    Point2 = points[2],
                    Point3 = points[3],
                };
            }
            myPathSegmentCollection.Add(segment);


            myPathFigure.Segments = myPathSegmentCollection;

            var myPathFigureCollection = new PathFigureCollection { myPathFigure };

            return new PathGeometry { Figures = myPathFigureCollection };

         
        }


        //private static Point2D GetPoint(Vector2D a, Vector2D b) => new Point2D(a.X * b.X, a.Y * b.Y);




        public static PathGeometry GetArcGeometry(Point start, Point end, double r)=>
         new PathGeometry
            {
                Figures = new PathFigureCollection
                 (
                     new[]
                     {
                        new PathFigure
                        {
                            StartPoint=start,
                            Segments=new PathSegmentCollection(
                                new []{
                                    new ArcSegment
                                    {
                                        SweepDirection=SweepDirection.Clockwise,
                                        Point=end,
                                        Size=new Size(r, r)
                                    }
                                })
                        }
                     }
                     )

            };



        //var controlPoints = new List<Point2D>();
        //double diff = (lineB.Point2D.Y - lineA.Point2D.X) / 3.0;
        //double xDiff = Math.Abs(lineA.Point2D.X - lineB.Point2D.X);
        //double yDiff = Math.Abs(lineA.Point2D.Y - lineB.Point2D.Y);
        //double dist = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        //double xOffset = dist / 2;
        //double yOffset = dist / 2;
        //Vector a = new Vector(xOffset, yOffset);

        //Point2D pt2 = GetPoint(a, lineA.Vector);
        //pt2 = new Point2D(lineA.Point2D.X + diff, lineA.Point2D.Y);
        //Point2D pt3 = GetPoint(a, lineB.Vector);
        //pt3 = new Point2D(lineB.Point2D.X - diff, lineB.Point2D.Y); 






        //  public static PathLine PathLine { get; set; }

        //private static PathCalculator instance;

        //private PathCalculator() { }

        //public static PathCalculator Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new PathCalculator();
        //        }
        //        return instance;
        //    }
        //}
        ////public PathFinderPoints(PathLine pathLine)
        ////{
        ////    PathLine = pathLine;
        ////}


    }
}


