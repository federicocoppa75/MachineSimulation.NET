using Machine.Data.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Machine.Views.Converters
{
    public class ToolToPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parName = parameter as string;
            var tool = value as ITool;

            switch (parName)
            {
                case "TotalDiameter":   return tool.GetTotalDiameter();
                case "TotalLength":     return tool.GetTotalLength();
                case "ToolType":        return tool.GetType().Name;
                default:                return null;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
