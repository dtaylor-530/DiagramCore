using System;
using System.Collections.Generic;
using System.Text;

namespace GeometryCore
{
    public interface IMessage
    {
        object From { get;  }

        object To { get;  }

        object Key { get; }

        object Content { get;  }


    }
}
