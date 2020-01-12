
using GeometryCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private bool canChange = true;
        private int size;

        public const int MinSize = 50;
        public const int MaxSize = 300;

        public NodeViewModel() : this(100)
        {
        }

        public NodeViewModel(int size)
        {
            Command = new SelectCommand(this);
            DragCommand = new DragCommand(this);
            ReSizeCommand = new ReSizeCommand(this);
            Size = size;
            this.PropertyChanged += NodeViewModel_PropertyChanged;
        }

        private void NodeViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NodeViewModel.Size))
            {
                var diff = Size - OldSize;
                this.X += (int)(diff / 2d);
                this.Y -= (int)(diff / 2d);
            }
        }

        public NodeViewModel(int x, int y, int size = 50) : this(size)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get => x;
            set { if (x != value) { OldX = x; this.x = value; RaisePropertyChanged(); } }
        }

        public int Y
        {
            get => y;
            set { if (y != value) { OldY = y; this.y = value; RaisePropertyChanged(); } }
        }

        public int Size
        {
            get => size;
            set { if (size != value) { OldSize = size; this.size = value; RaisePropertyChanged(); } }
        }

        public bool IsSelected
        {
            get => isSelected;
            set { if (isSelected != value) { this.isSelected = value; RaisePropertyChanged(); } }
        }

        public bool CanChange
        {
            get => canChange;
            set { if (canChange != value) { this.canChange = value; RaisePropertyChanged(); } }
        }



        public object Object { get; set; } = "Node";


        [Browsable(false)]
        public int OldX { get; private set; }

        [Browsable(false)]
        public int OldY { get; private set; }

        [Browsable(false)]
        public int OldSize { get; private set; }

        [Browsable(false)]
        public ICommand Command { get; }

        [Browsable(false)]
        public ICommand DragCommand { get; }

        [Browsable(false)]
        public ICommand ReSizeCommand { get; }


        public ObservableCollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();

        public object Key { get; set; }

        #region propertychanged

        [Browsable(false)]
        public event PropertyChangedEventHandler PropertyChanged;
        //public event Action<IMessage> MessageChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

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

        public virtual void NextChange(IMessage message)
        {
            if (message.Key.ToString() != string.Empty)
            {
                Messages.Add(message);
                this.RaisePropertyChanged(string.Empty);
            }
        }
        //public void NextMessage(IMessage message)
        //{
        //    this.MessageChanged(new Message(this.Object, null, message.Content));
        //}
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
            return pvm.CanChange;
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

    public class ReSizeCommand : ICommand
    {
        private NodeViewModel pvm;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return pvm.CanChange;
        }

        public ReSizeCommand(NodeViewModel pvm)
        {
            this.pvm = pvm;
        }
        public void Execute(object parameter)
        {

            double sizeDelta = ((-(parameter as DragDeltaEventArgs).HorizontalChange + (parameter as DragDeltaEventArgs).VerticalChange) / 100d);

            int roundedSizeDelta = (int)Math.Round(sizeDelta, 0, MidpointRounding.AwayFromZero);
            if (((pvm as NodeViewModel).Size < NodeViewModel.MaxSize || roundedSizeDelta < 0) &&
                ((pvm as NodeViewModel).Size > NodeViewModel.MinSize || roundedSizeDelta > 0))
            {
                (pvm as NodeViewModel).Size += roundedSizeDelta;

                (parameter as DragDeltaEventArgs).Handled = true;
            }
        }
    }
}
