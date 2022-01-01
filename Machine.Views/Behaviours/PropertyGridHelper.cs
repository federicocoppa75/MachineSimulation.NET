using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Machine.Views.Behaviours
{
    internal static class PropertyGridHelper
    {
        #region MouseRightButtonUp

        public static ICommand GetPropertyValueChanged(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(PropertyValueChangedProperty);
        }

        public static void SetPropertyValueChanged(DependencyObject obj, ICommand value)
        {
            obj.SetValue(PropertyValueChangedProperty, value);
        }

        public static readonly DependencyProperty PropertyValueChangedProperty =
            DependencyProperty.RegisterAttached("PropertyValueChanged",
            typeof(ICommand),
            typeof(PropertyGridHelper),
            new PropertyMetadata(null, new PropertyChangedCallback(PropertyValueChangedEnter)));

        private static void PropertyValueChangedEnter(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as PropertyGrid;

            if (element != null) element.PropertyValueChanged += Element_PropertyValueChanged; ;
        }

        private static void Element_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            var element = sender as FrameworkElement;
            var command = GetPropertyValueChanged(element);

            if (command != null && command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        #endregion

    }
}
