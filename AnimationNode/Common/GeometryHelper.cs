﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AnimationNode
{



    public static class GeometryHelper
    {
        public static double GetLength(this Geometry geo)
        {
            PathGeometry path = geo.GetFlattenedPathGeometry();

            double length = 0.0;

            foreach (PathFigure pf in path.Figures)
            {
                Point start = pf.StartPoint;

                foreach (PathSegment seg in pf.Segments)
                {
                    if (seg is PolyLineSegment)
                        foreach (Point point in (seg as PolyLineSegment).Points)
                        {
                            length += Distance(start, point);
                            start = point;
                        }
                    else if (seg is LineSegment)
                    {
                        var point=(seg as LineSegment).Point;
                        length += Distance(start, point);
                        start = point;
                    }  
                }
            }

            return length;
        }

        private static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}
