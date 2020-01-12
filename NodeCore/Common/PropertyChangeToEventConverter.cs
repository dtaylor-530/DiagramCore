using Microsoft.Xaml.Behaviors;
using System;
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
            command.Execute(null);
        }

    }
}
