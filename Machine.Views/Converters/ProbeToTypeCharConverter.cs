using Machine.ViewModels.Interfaces.Probing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Machine.Views.Converters
{
    [ContentProperty("Values")]
    class ProbeToTypeCharConverter : IValueConverter
    {
        public List<ProbeToTypeCharConverterItem> Values { get; set; } = new List<ProbeToTypeCharConverterItem>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = null;

            if (value is IProbe v)
            {
                var interfaces = v.GetType().GetInterfaces();


                foreach (var item in interfaces)
                {
                    var end = false;

                    foreach (var option in Values)
                    {
                        if(string.Compare(item.Name, option.When) == 0)
                        {
                            result = option.Then;
                            end = true;
                            break;
                        }
                    }

                    if (end) break;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ContentProperty("Then")]
    class ProbeToTypeCharConverterItem
    {
        public string When { get; set; }
        public object Then { get; set; }
    }
}
