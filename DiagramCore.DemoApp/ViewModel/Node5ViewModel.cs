using GeometryCore;
using NodeCore;
using OnTheFlyStats;

namespace DiagramCore.DemoApp
{
    public class Node5ViewModel : NodeViewModel
    {
        private Stats populationStats;

        public Node5ViewModel(int x, int y, object key) : base(x, y, key)
        {
            CanChange = true;
            populationStats = new Stats();
        }

        public override void NextMessage(IMessage message)
        {
            if (message.Key.ToString() == nameof(NodeViewModel.Y))
            {
                (int val, double weight) = ((int, double))message.Content;


                populationStats.Update(val);



                this.Y = (int)populationStats.Average;

                InwardMessages.Add(message);
            }

            else
            {
                base.NextMessage(message);
            }

        }
    }
}
