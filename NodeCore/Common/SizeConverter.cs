using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Data;

namespace NodeCore.Common
{
    //public class SizeConverter : IValueConverter
    //{
    //    Type type = typeof(NodeViewModel);

    //    Dictionary<string, PropertyInfo> dict = new Dictionary<string, PropertyInfo>();

    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        NodeViewModel val = value as NodeViewModel;

    //        if (dict.ContainsKey(parameter.ToString()) == false)
    //            dict[parameter.ToString()] = type.GetProperty(parameter.ToString());

    //        double d = (double)dict[parameter.ToString()].GetValue(val);

    //        return d + val.Size / 2;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
