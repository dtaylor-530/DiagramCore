
using FluentValidation;
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
    public class NodeViewModelValidator : AbstractValidator<NodeViewModel>
    {
        public NodeViewModelValidator()
        {
            RuleFor(customer => customer.Y).GreaterThanOrEqualTo(a => a.YThreshold + a.OldY);
            RuleFor(customer => customer.X).GreaterThanOrEqualTo(a => a.XThreshold + a.OldX);
            RuleFor(customer => customer.Size).LessThanOrEqualTo(a => NodeViewModel.MaxSize);
            RuleFor(customer => customer.Size).GreaterThanOrEqualTo(a => NodeViewModel.MinSize);
        }
    }

    public class NodeViewModel : ReactiveUI.FluentValidation.ReactiveValidationObject, INode, IEquatable<NodeViewModel>
    {
        private int x;
        private int y;
        private bool isSelected;
        private bool canChange = true;
        private int size;
        private int yThreshold = 10;
        private int xThreshold = 10;
        private int sizeThreshold;

        public const int MinSize = 50;
        public const int MaxSize = 300;

        public NodeViewModel(object key) : this(key, 50)
        {
        }

        public NodeViewModel(object key, int size) : base(new NodeViewModelValidator())
        {
            Command = new SelectCommand(this);
            DragCommand = new DragCommand(this);
            ReSizeCommand = new ReSizeCommand(this);
            Size = size;
            Key = key;
            this.PropertyChanged += NodeViewModel_PropertyChanged;
        }

        public NodeViewModel(int x, int y, object key, int size = 50) : this(key, size)
        {
            this.X = x;
            this.Y = y;
        }

        private void NodeViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Size))
            {
                var diff = Size - OldSize;
                this.X += (int)(diff / 2d);
                this.Y -= (int)(diff / 2d);
            }
            if (e.PropertyName != string.Empty)
            {
                var property = sender.GetType().GetProperty(e.PropertyName).GetValue(sender);
                //if (e.PropertyName == nameof(Y))
                //{
                //    property = (this.Y, this.Size);
                //}
                var message = new Message(this.Key, null, e.PropertyName, property);
                Messages.Add(message);
            }
        }

        public int X
        {
            get => x;
            set { OldX = x; RaiseAndValidateAndSetIfChanged(ref x, value); }
        }

        public int Y
        {
            get => y;
            set { OldY = y; RaiseAndValidateAndSetIfChanged(ref y, value); }
        }

        public int Size
        {
            get => size;
            set { OldSize = size; RaiseAndValidateAndSetIfChanged(ref size, value); }
        }


        public int XThreshold
        {
            get => xThreshold;
            set => RaiseAndValidateAndSetIfChanged(ref xThreshold, value);
        }

        public int YThreshold
        {
            get => yThreshold;
            set => RaiseAndValidateAndSetIfChanged(ref yThreshold, value);
        }

        public int SizeThreshold
        {
            get => sizeThreshold;
            set => RaiseAndValidateAndSetIfChanged(ref sizeThreshold, value);
        }


        public bool IsSelected
        {
            get => isSelected;
            set => RaiseAndValidateAndSetIfChanged(ref isSelected, value);
        }

        public bool CanChange
        {
            get => canChange;
            set => RaiseAndValidateAndSetIfChanged(ref canChange, value);
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


        public ObservableCollection<IMessage> InwardMessages { get; } = new ObservableCollection<IMessage>();

        public ObservableCollection<INode> Nodes { get; } = new ObservableCollection<INode>();


        public ICollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();

        public object Key { get; set; }


        #region equality

        public bool Equals([AllowNull] NodeViewModel other)
        {
            return other != null && this.X == other.X && this.Y == other.Y && this.Size == other.Size && this.Key == other.Key;
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

        public virtual void NextMessage(IMessage message)
        {
            if (message.Key.ToString() != string.Empty)
            {
                InwardMessages.Add(message);
                this.RaisePropertyChanged(string.Empty);

                var node = Nodes.SingleOrDefault(a => a.Key.Equals(message.From));

                if (node == null)
                {
                    node = new NodeViewModel(message.From);
                    Nodes.Add(node);
                }

                typeof(NodeViewModel).GetProperty(message.Key.ToString()).SetValue(node, message.Content);

            }
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
