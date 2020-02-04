using GeometryCore;
using NodeCore;
using OnTheFlyStats;

namespace DiagramCore.DemoApp
{
    public class Node5ViewModel : NodeViewModel
    {
        private Stats stats;

        public double Mean => stats.Average;

        public double StandardDeviation => stats.PopulationStandardDeviation;


        public Node5ViewModel(int x, int y, object key) : base(x, y, key)
        {
            CanChange = false;
            
            //SuppressChange = true;
            stats = new Stats();
        }

        public override void NextMessage(IMessage message)
        {
            if (message.Key.ToString() == nameof(NodeViewModel.Y))
            {
                (int val, double weight) = ((int, double))message.Content;

                stats.Update(val);

                this.Y = (int)stats.Average;

                InwardMessages.Add(message);
            }
            else
            {
                base.NextMessage(message);
            }
        }
    }
}
