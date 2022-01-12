using Machine.ViewModels.UI;
using System;
using System.Windows;

namespace Machine.Views.UI
{
    public class SimpleExceptionObserver : IExceptionObserver
    {
        public void NotifyException(Exception exception)
        {
            MessageBox.Show(exception.Message, $"Error ({exception.GetType().Name})", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
