using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Machine._3D.Views.Converters
{
    class AxIdToLabelCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int id)
            {
                switch (id)
                {
                    case (1):       return "X";
                    case (2):       return "U";
                    case (101):     return "Y";
                    case (201):     return "V";
                    case (102):     return "Z";
                    case (202):     return "W";
                    case (112):     return "A";
                    case (212):     return "B";
                    default:        return value;
                }
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
