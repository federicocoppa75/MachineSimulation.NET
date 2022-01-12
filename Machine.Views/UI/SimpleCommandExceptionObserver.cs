using Machine.ViewModels.Base;
using System;
using System.Windows;

namespace Machine.Views.UI
{
    public class SimpleCommandExceptionObserver : ICommandExceptionObserver
    {
        public void NotifyException(Exception exception)
        {
            MessageBox.Show(exception.Message, $"Error ({exception.GetType().Name})", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
