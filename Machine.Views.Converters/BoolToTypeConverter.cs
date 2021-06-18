using System;
using System.Globalization;
using System.Windows.Data;

namespace Machine.Views.Converters
{
    public class BoolToTypeConverter<T> : IValueConverter
    {
        public T ValueForTrue { get; set; }

        public T ValueForFalse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value is bool v) && v) ? ValueForTrue : ValueForFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
