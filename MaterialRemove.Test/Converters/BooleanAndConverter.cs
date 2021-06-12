using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MaterialRemove.Test.Converters
{
    class BooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if((values != null) && (values.Count() > 0))
            {
                return values.All((o) =>
                               {
                                   if(o is bool b)
                                   {
                                       return b;
                                   }
                                   else
                                   {
                                       return false;
                                   }
                               });
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
