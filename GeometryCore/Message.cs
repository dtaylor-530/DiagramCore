using System;
using System.Collections.Generic;
using System.Text;

namespace GeometryCore
{
    public struct Message : IMessage
    {
        public Message(object from, object to, object key, object content)
        {
            From = from;
            To = to;
            Key = key;
            Content = content;
            Time = DateTime.Now;
        }

        public object From { get; }

        public object To { get;  }

        public object Key { get; }

        public object Content { get;  }

        public DateTime Time { get; }
    }

    public struct Message<T> : IMessage
    {
        public Message(object from, object to, object key, T content)
        {
            From = from;
            To = to;
            Key = key;
            ContentAsType = content;
            Time = DateTime.Now;
        }

        public object From { get; }

        public object To { get; }

        public object Key { get; }

        public T ContentAsType { get; }


        public object Content => ContentAsType;

        public DateTime Time { get; }
    }

}
