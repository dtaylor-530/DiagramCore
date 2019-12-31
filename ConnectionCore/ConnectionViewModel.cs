using GeometryCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionCore
{
    public class ConnectionViewModel : INotifyPropertyChanged
    {
        private INode pvm1;
        private INode pvm2;

        public ConnectionViewModel(INode pvm1, INode pvm2)
        {
            this.pvm1 = pvm1;
            this.pvm2 = pvm2;
            pvm1.PropertyChanged += PropertyChanged1;
            pvm2.PropertyChanged += PropertyChanged2;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void PropertyChanged1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(pvm1.X))
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.X1)));
            if (e.PropertyName == nameof(pvm1.Y))
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Y1)));
        }

        private void PropertyChanged2(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(pvm1.X))
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.X2)));
            if (e.PropertyName == nameof(pvm1.Y))
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Y2)));
        }


        public double X1 { get => pvm1.X + pvm1.Size/2d; }

        public double Y1 { get => pvm1.Y + pvm1.Size / 2d; }

        public double X2 { get => pvm2.X + pvm1.Size / 2d; }

        public double Y2 { get => pvm2.Y + +pvm1.Size / 2d; }

    }
}
