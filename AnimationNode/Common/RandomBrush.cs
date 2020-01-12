﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AnimationNode
{

    public static class RandomBrush
    {
        static IEnumerator<Brush> enumerator;
        
        static RandomBrush()
        {
            enumerator = BrushHelper.GetBrushes().GetEnumerator();
        }

        public static Brush Brush
        {
            get
            {
                while (!enumerator.MoveNext())
                    enumerator = BrushHelper.GetBrushes().GetEnumerator();
                return enumerator.Current;
            }
        }


    }


    public static class BrushHelper
    {
        public static IEnumerable<Brush> GetBrushes()
        {
            var props = typeof(Brushes).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (var propInfo in props)
            {
                yield return (Brush)propInfo.GetValue(null, null);
            }
        }

    }
}
