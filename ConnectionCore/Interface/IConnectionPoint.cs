
using GeometryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace ConnectionCore
{

    public interface IConnectionPoint
    {
        Point Position { get; set; }

        Side Side { get; set; }
    }

}

