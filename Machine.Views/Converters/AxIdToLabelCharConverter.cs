using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Machine.Views.Converters
{
    [ContentProperty("Values")]
    class AxIdToLabelCharConverter : IValueConverter
    {
        public List<AxIdToLabelCharConverterItem> Values { get; set; } = new List<AxIdToLabelCharConverterItem>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = null;

            if (value is int v)
            {
                foreach (var item in Values)
                {
                    if (item.When == v)
                    {
                        result = item.Then;
                        break;
                    }
                }
            }

            if (result == null) result = value;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ContentProperty("Then")]
    class AxIdToLabelCharConverterItem
    {
        public int When { get; set; }
        public object Then { get; set; }
    }
}
