using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DiagramCore
{
    public class PointViewer : Control
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(PointViewer), new PropertyMetadata(0d));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(PointViewer), new PropertyMetadata(0d));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(PointViewer), new PropertyMetadata());


        static PointViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PointViewer), new FrameworkPropertyMetadata(typeof(PointViewer)));
        }

        public PointViewer()
        {

        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }


        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }




    }
}
