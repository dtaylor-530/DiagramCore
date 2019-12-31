using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeometryCore
{
    public static class SideExtension
    {

        public static Side FromString(this string value)
        {
            switch (value.ToLower())
            {
                case "right": return Side.Right;
                case "left": return Side.Left;
                case "bottom": return Side.Bottom;
                case "top": return Side.Top;

                default: return Side.None;
            }


        }


        public static Side ToReverse(this Side value)
        {
            switch (value)
            {
                case Side.Left: return Side.Right;
                case Side.Right: return Side.Left;
                case Side.Top: return Side.Bottom;
                case Side.Bottom: return Side.Top;

                default: return Side.None;
            }


        }

        public static double ToAngle(this Side value)
        {
            switch (value)
            {
                case Side.Left: return 270;
                case Side.Right: return 90;
                case Side.Top: return 0;
                case Side.Bottom: return 180;

                default: return 0;
            }
        }

        public static Side FromAngle(this double angle)
        {
            if (angle > -45 & angle <= 45) return Side.Bottom;
            else if (angle > 45 & angle <= 135) return Side.Right;
            else if (angle < -135 || angle >= 135) return Side.Top;
            else if (angle < -45 & angle >= -135) return Side.Left;
            return Side.None;

        }

        public static Vector2D ToVector(this Side value)
        {
            switch (value)
            {
                case Side.Left: return new Vector2D(1, 0);
                case Side.Right: return new Vector2D(-1, 0);
                case Side.Top: return new Vector2D(0, 1);
                case Side.Bottom: return new Vector2D(0, -1);
                default: return new Vector2D(0, 0);
            }
        }

        // gets the relative side of an element to another
        // so if A is to the left of B, returns Side.Left
        public static Side RelativeSide(Point2D pointA, Point2D pointB)
        {

            Vector2D vec = (pointB - pointA);

            double ang = vec.AngleTo(new Vector2D(0, 1)).Degrees;
            Side side = FromAngle(ang);
            return side;

        }

        // gets the relative side of an element to another
        // so if A is to the left of B, returns Side.Left
        public static Side RelativeHeight(Point2D pointA, Point2D pointB)
        {
            Side y = Side.None;

            if (pointA.Y != pointB.Y)
                y = (pointA.Y > pointB.Y) ? Side.Top : Side.Bottom;


            return y;

        }

    }

}
