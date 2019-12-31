using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ConnectionCore
{
    public class ConnectionPointToLineConverter : IMultiValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    IConnectionPoint x = value as IConnectionPoint;
        //    return new Line
        //    {
        //        Vector = x.Side.ToVector(),
        //        Point = x.Position
        //    };
        //}

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            var vector = (Vector2D)values[0];
            var point = (Point)values[1];

            var diff = point.X + vector.X;
            var diffY = point.Y + vector.Y;
            return new Line()
            {
                X1=point.X,
                X2 = diff,
                Y1= point.Y,
                Y2 =diffY
          
            };
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
