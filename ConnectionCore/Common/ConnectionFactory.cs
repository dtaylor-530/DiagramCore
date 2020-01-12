using GeometryCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectionCore
{
    public class ConnectionFactory
    {
        public static IEnumerable<ConnectionViewModel> Build(INode node, int delay=0, params INode[] nodes)
        {
            foreach(var n in nodes)
            {
                var conn = new ConnectionViewModel(node, n) { Delay = delay };
                yield return conn;
            }

        }

    }
}