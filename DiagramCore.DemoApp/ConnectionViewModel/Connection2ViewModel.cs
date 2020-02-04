using ConnectionCore;
using GeometryCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagramCore.DemoApp
{
    class Connection2ViewModel:ConnectionViewModel
    {
        public Connection2ViewModel(INode node1, INode node2, bool isNegative , bool birectional = true) : base(node1, node2, birectional)
        {
            this.IsNegative = isNegative;
        }

        public bool IsNegative { get; }

        protected override IMessage Modify(IMessage message, int size)
        {
            if (message.Key.Equals(nameof(node1.Y)))
            {
                int val = System.Convert.ToInt32(message.Content);

                val = IsNegative ? -val : val;
                // the decay (like half-life)
                var weight = size * Math.Exp(-decayFactor * XDistance);

                message = new Message(message.From, message.To, message.Key, (val, weight));

            }
            return message;
        }
    }
}
