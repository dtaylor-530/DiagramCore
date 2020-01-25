using GeometryCore;
using NodeCore;

namespace DiagramCore.DemoApp
{
    public class Node4ViewModel : NodeViewModel
    {

        public Node4ViewModel(int x, int y, object key) : base(x, y, key)
        {
            CanChange = false;

        }
        public override void NextMessage(IMessage message)
        {
            if (message.Key.ToString() == nameof(NodeViewModel.Y))
            {
                (int val, double weight) = ((int, double))message.Content;


                var content = (val * weight + this.Y * this.Size) / (weight + this.Size);

                this.Y = (int) content;
                
                InwardMessages.Add(message);
            }
            
            else
            {
                base.NextMessage(message);
            }
            
        }
    }
}
