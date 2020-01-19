using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NodeCore
{
    public class FadeControl : Control
    {
        private Ellipse ellipse;

        public override void OnApplyTemplate()
        {
            ellipse = this.GetTemplateChild("PART_Ellipse") as Ellipse;
            ellipse.Fill = new SolidColorBrush(Color);
            if (FadeIn)
            {
                ellipse.Opacity = 0;
            }
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(FadeControl), new PropertyMetadata(Colors.Red));

        public ICommand FadeCommand
        {
            get { return (ICommand)GetValue(FadeCommandProperty); }
            set { SetValue(FadeCommandProperty, value); }
        }

        public static readonly DependencyProperty FadeCommandProperty = DependencyProperty.Register("FadeCommand", typeof(ICommand), typeof(FadeControl), new PropertyMetadata(default(ICommand)));


        static FadeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FadeControl), new FrameworkPropertyMetadata(typeof(FadeControl)));
        }

        public FadeControl()
        {
            FadeCommand = new FadeCommand(this);
        }

        public void Fade()
        {
            if (this.ellipse != null)
            {
                RunStoryBoard(this.ellipse, FadeIn);
            }
        }


        public bool FadeIn
        {
            get { return (bool)GetValue(FadeInProperty); }
            set { SetValue(FadeInProperty, value); }
        }

        public static readonly DependencyProperty FadeInProperty =
            DependencyProperty.Register("FadeIn", typeof(bool), typeof(FadeControl), new PropertyMetadata(true));

        public static void RunStoryBoard(DependencyObject element, bool fadeIn)
        {
            Storyboard storyboard = new Storyboard();
            TimeSpan duration = TimeSpan.FromMilliseconds(500); //


            DoubleAnimation fadeInAnimation = new DoubleAnimation()
            { From = fadeIn ? 0 : 1, To = fadeIn ? 1 : 0, Duration = new Duration(duration) };

            DoubleAnimation fadeOutAnimation = new DoubleAnimation()
            { From = fadeIn ? 1 : 0, To = fadeIn ? 0 : 1, Duration = new Duration(duration) };
            //fadeOutAnimation.BeginTime = TimeSpan.FromSeconds(5);

            Storyboard.SetTarget(fadeInAnimation, element);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity", fadeIn ? 0 : 1));
            storyboard.Children.Add(fadeInAnimation);


            Storyboard.SetTarget(fadeOutAnimation, element);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity", fadeIn ? 1 : 0));
            storyboard.Children.Add(fadeOutAnimation);
            storyboard.Begin();
        }
    }

    internal class FadeCommand : ICommand
    {
        private FadeControl fadeControl;

        public event EventHandler CanExecuteChanged;

        public FadeCommand(FadeControl fadeControl) => this.fadeControl = fadeControl;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            fadeControl.Fade();
        }
    }
}

