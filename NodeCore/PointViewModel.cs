
using GeometryCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NodeCore
{
    public class PointViewModel : INode, IEquatable<PointViewModel>
    {
        private int x;
        private int y;
        private bool isSelected;

        public int X
        {
            get => x; 
            set
            {
                if (x != value)
                {
                    OldX = x;
                    this.x = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X)));
                }
            }
        }

        public int Y
        {
            get => y;
            set
            {
                if (y != value)
                {
                    OldY = y;
                    this.y = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y)));
                }
            }
        }

        [Browsable(false)]
        public int OldX { get; private set; }

        [Browsable(false)]
        public int OldY { get; private set; }
     

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    this.isSelected = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public int Size { get; }


        public object Object { get; set; }

        public PointViewModel()
        {
            Command = new SelectCommand(this);
            DragCommand = new DragCommand(this);
            Size = 100;
        }

        [Browsable(false)]
        public ICommand Command { get; }


        [Browsable(false)]
        public ICommand DragCommand { get; }

        [Browsable(false)]
        public event PropertyChangedEventHandler PropertyChanged;


        #region equality
        
        public bool Equals([AllowNull] PointViewModel other)
        {
            return other !=null && this.X == other.X && this.Y == other.Y && this.Size == other.Size;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(x);
            hash.Add(y);
            hash.Add(isSelected);
            hash.Add(Size);

            return hash.ToHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PointViewModel);
        }

        public static bool operator ==(PointViewModel left, PointViewModel right)
        {
            return EqualityComparer<PointViewModel>.Default.Equals(left, right);
        }

        public static bool operator !=(PointViewModel left, PointViewModel right)
        {
            return !(left == right);
        }

        #endregion equality

        public override string ToString()
        {
            return $"{x} {y} {Size}";
        }

    }



    public class SelectCommand : ICommand
    {
        private PointViewModel pvm;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public SelectCommand(PointViewModel pvm)
        {
            this.pvm = pvm;
        }
        public void Execute(object parameter)
        {
            if (pvm != null)
                (pvm).IsSelected = true;
        }
    }

    public class DragCommand : ICommand
    {
        private PointViewModel pvm;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public DragCommand(PointViewModel pvm)
        {
            this.pvm = pvm;
        }
        public void Execute(object parameter)
        {

            (pvm as PointViewModel).X += (int)(parameter as DragDeltaEventArgs).HorizontalChange;
            (pvm as PointViewModel).Y += (int)(parameter as DragDeltaEventArgs).VerticalChange;
            (parameter as DragDeltaEventArgs).Handled = true;
        }
    }
}
