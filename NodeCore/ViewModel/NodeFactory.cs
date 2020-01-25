using GeometryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeCore
{
    public class NodeFactory
    {

        public static IEnumerable<INode> BuildCircle(int x, int y, int radius, int number)
        {
            return
                SelectCircleCoordinates(x, y, radius, number)
                .Select(a => new NodeViewModel((int)a.x,(int) a.y));
        }


        public static IEnumerable<(double x, double y)> SelectCircleCoordinates(int x0, int y0, int radius, int number)
        {
            double division = 360.0 / number;

            for (int i = 0; i < number; i++)
            {
                double a = division * (i + 1);
                double x = x0 + radius * Math.Cos(Math.PI * a / 180.0);
                double y = y0 + radius * Math.Sin(Math.PI * a / 180.0);
                yield return (x, y);
            }
        }

        public static IEnumerable<(double x, double y)> SelectSineCoordinates(int x0, int y0, int radius, int number)
        {
            double division = 360.0 / number;

            for (int i = 0; i < number; i++)
            {
                double a = division * (i + 1);
                double x = a;
                double y = y0 + radius * Math.Sin(Math.PI * a / 180.0);
                yield return (x, y);
            }
        }
    }
}