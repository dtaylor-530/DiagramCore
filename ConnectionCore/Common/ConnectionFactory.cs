using GeometryCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectionCore
{
    public class ConnectionFactory
    {
        public static IEnumerable<ConnectionViewModel> Build(INode node, params INode[] nodes)
        {
            foreach(var n in nodes)
            {
                var conn = new ConnectionViewModel(node, n);
                yield return conn;
            }

        }

    }
}