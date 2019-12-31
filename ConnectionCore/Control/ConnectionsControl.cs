using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ConnectionCore
{
    public class ConnectionsControl:ItemsControl
    {

        static ConnectionsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConnectionsControl), new FrameworkPropertyMetadata(typeof(ConnectionsControl)));
          
        }

    }
}
