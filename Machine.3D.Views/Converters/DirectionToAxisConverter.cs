using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using Machine.Data.Enums;

namespace Machine._3D.Views.Converters
{
    class DirectionToAxisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is LinkDirection direction)
            {
                switch (direction)
                {
                    case LinkDirection.X:
                        return new Vector3D(1.0, 0.0, 0.0);
                    case LinkDirection.Y:
                        return new Vector3D(0.0, 1.0, 0.0);
                    case LinkDirection.Z:
                        return new Vector3D(0.0, 0.0, 1.0);
                    default:
                        throw new ArgumentOutOfRangeException();                        
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
