using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GeometryCore
{
    public interface INode : INotifyPropertyChanged
    {
        int X { get; }

        int Y { get; }

        int Size { get; }
    }
}
