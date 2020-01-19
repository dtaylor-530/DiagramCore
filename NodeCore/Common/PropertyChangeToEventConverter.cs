using Microsoft.Xaml.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace NodeCore
{


    public class PropertyEventToCommandBehavior : Behavior<FrameworkElement>
    {
        private ICommand command;

        protected override void OnAttached()
        {
            var command = AssociatedObject.GetType()
                .GetProperties()
                .Single(a => a.PropertyType == typeof(ICommand)).GetValue(AssociatedObject) as ICommand;
            var dataContext = (AssociatedObject.DataContext) as INotifyPropertyChanged;

            dataContext.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            this.command = command;


        }
        private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Properties?.Cast<string>().Contains(e.PropertyName) ?? true)
                command.Execute(null);
        }



        public IEnumerable Properties
        {
            get { return (IEnumerable)GetValue(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Properties.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertiesProperty =
            DependencyProperty.Register("Properties", typeof(IEnumerable), typeof(PropertyEventToCommandBehavior), new PropertyMetadata(null));




    }

}
