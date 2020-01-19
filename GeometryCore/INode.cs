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

        object Key { get; }

        //event Action<IMessage> MessageChanged;
        ICollection<IMessage> Messages { get; }

        void NextMessage(IMessage message);
    }
}
