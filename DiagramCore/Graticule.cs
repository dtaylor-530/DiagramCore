using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DiagramCore
{
    public class Graticule : Canvas
    {

        static Graticule()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Graticule), new FrameworkPropertyMetadata(typeof(Graticule)));
        }
    }
}
