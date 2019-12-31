using System.Collections.Generic;
using System.Windows;
using GeometryCore;
using MathNet.Spatial.Euclidean;

/// <summary>
/// https://stackoverflow.com/questions/9247564/convert-bezier-curve-to-polygonal-chain
///answered Jun 2 '18 at 3:35
///batpox
/// </summary>

namespace ConnectionCore
{
    public class Bezier
    {
        public Point2D P1;   // Begin Point2D
        public Point2D P2;   // Control Point2D
        public Point2D P3;   // Control Point2D
        public Point2D P4;   // End Point2D

        // Made these global so I could diagram the top solution
        public Line2D L12;
        public Line2D L23;
        public Line2D L34;

        public Point2D P12;
        public Point2D P23;
        public Point2D P34;

        public Line2D L1223;
        public Line2D L2334;

        public Point2D P123;
        public Point2D P234;

        public Line2D L123234;
        public Point2D P1234;

        public Bezier(Point2D p1, Point2D p2, Point2D p3, Point2D p4)
        {
            P1 = p1; P2 = p2; P3 = p3; P4 = p4;
        }

        /// <summary>
        /// Consider the classic Casteljau diagram
        /// with the bezier Point2Ds p1, p2, p3, p4 and Line2Ds l12, l23, l34
        /// and their midPoint2D of Line2D l12 being p12 ...
        /// and the Line2D between p12 p23 being L1223
        /// and the midPoint2D of Line2D L1223 being P1223 ...
        /// </summary>
        /// <param name="Line2Ds"></param>
        public void SplitBezier(List<Line2D> Line2Ds)
        {
            Line2Ds = Line2Ds ?? new List<Line2D>();

            L12 = new Line2D(this.P1, this.P2);
            L23 = new Line2D(this.P2, this.P3);
            L34 = new Line2D(this.P3, this.P4);

            P12 = L12.MidPoint();
            P23 = L23.MidPoint();
            P34 = L34.MidPoint();

            L1223 = new Line2D(P12, P23);
            L2334 = new Line2D(P23, P34);

            P123 = L1223.MidPoint();
            P234 = L2334.MidPoint();

            L123234 = new Line2D(P123, P234);

            P1234 = L123234.MidPoint();

            if (CurveIsFlat())
            {
                Line2Ds.Add(new Line2D(this.P1, this.P4));
                return;
            }
            else
            {
                Bezier bz1 = new Bezier(this.P1, P12, P123, P1234);
                bz1.SplitBezier(Line2Ds);

                Bezier bz2 = new Bezier(P1234, P234, P34, this.P4);
                bz2.SplitBezier(Line2Ds);
            }

            return;
        }

        /// <summary>
        /// Check if Point2Ds P1, P1234 and P2 are coLine2Dar (enough).
        /// This is very simple-minded algo... there are better...
        /// </summary>
        /// <returns></returns>
        public bool CurveIsFlat()
        {
            double t1 = (P2.Y - P1.Y) * (P3.X - P2.X);
            double t2 = (P3.Y - P2.Y) * (P2.X - P1.X);

            double delta = System.Math.Abs(t1 - t2);

            return delta < 0.1; // Hard-coded constant
        }




    }


}