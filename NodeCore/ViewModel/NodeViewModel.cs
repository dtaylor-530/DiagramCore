
using GeometryCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NodeCore
{
    public class NodeViewModel : INode, IEquatable<NodeViewModel>
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        public int Size { get; }


        public object Object { get; set; } = "Node";

        public NodeViewModel() : this(100)
        {

        }


        public NodeViewModel(int size)
        {
            Command = new SelectCommand(this);
            DragCommand = new DragCommand(this);
            Size = size;
        }

        public NodeViewModel(int x, int y, int size = 50):this(size)
        {
            this.x = x;
            this.y = y;
         
        }

        [Browsable(false)]
        public ICommand Command { get; }


        [Browsable(false)]
        public ICommand DragCommand { get; }

        [Browsable(false)]
        public event PropertyChangedEventHandler PropertyChanged;


        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region equality

        public bool Equals([AllowNull] NodeViewModel other)
        {
            return other != null && this.X == other.X && this.Y == other.Y && this.Size == other.Size;
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
            return this.Equals(obj as NodeViewModel);
        }

        public static bool operator ==(NodeViewModel left, NodeViewModel right)
        {
            return EqualityComparer<NodeViewModel>.Default.Equals(left, right);
        }

        public static bool operator !=(NodeViewModel left, NodeViewModel right)
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
        private NodeViewModel pvm;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public SelectCommand(NodeViewModel pvm)
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
        private NodeViewModel pvm;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public DragCommand(NodeViewModel pvm)
        {
            this.pvm = pvm;
        }
        public void Execute(object parameter)
        {

            (pvm as NodeViewModel).X += (int)(parameter as DragDeltaEventArgs).HorizontalChange;
            (pvm as NodeViewModel).Y += (int)(parameter as DragDeltaEventArgs).VerticalChange;
            (parameter as DragDeltaEventArgs).Handled = true;
        }
    }
}
