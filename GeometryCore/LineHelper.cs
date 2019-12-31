using MathNet.Spatial.Euclidean;
using System;

namespace GeometryCore
{

    public static class LineHelper
    {
        public static Point2D MidPoint(this Line2D line)
        {
            return MidPoint(line.StartPoint, line.EndPoint);
        }
        public static Point2D MidPoint(this Point2D p1, Point2D p2)
        {
            return new Point2D((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        public static Vector2D ToVector(this Line2D line)
        {
            return new Vector2D(line.EndPoint.X - line.StartPoint.X, line.EndPoint.Y - line.StartPoint.Y); ;
        }
    }

}
