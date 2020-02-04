using GeometryCore;
using NodeCore;

namespace DiagramCore.DemoApp
{
    public class Node2ViewModel : NodeViewModel
    {

        public Node2ViewModel(int x, int y, object key) : base(x, y, key)
        {
            CanChange = false;

        }
        public override void NextMessage(IMessage message)
        {
            if (message.Key.ToString() == nameof(NodeViewModel.Size))
            {
                var val = (int)message.Content;
                this.Size = (int)(((int)val) / 2d);
            }
            base.NextMessage(message);
        }
    }
}
