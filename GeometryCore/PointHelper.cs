using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeometryCore
{
    public static class PointHelper
    {
        public static double DetermineArcRadius(this Point2D start, Point2D end, double angle)
        {
            double sinA = Math.Sin(Math.PI * angle / 180d);

            double x = start.X - end.X;
            double y = start.Y - end.Y;
            double aa = x * x + y * y;
            double l = Math.Sqrt(aa);
            return l / (sinA * 2);
        }

    }
}
