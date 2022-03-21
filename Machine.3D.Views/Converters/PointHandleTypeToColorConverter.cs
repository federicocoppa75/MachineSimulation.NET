using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MVMIH = Machine.ViewModels.Interfaces.Handles;

namespace Machine._3D.Views.Converters
{
    internal class PointHandleTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((value != null) && (value is MVMIH.Type type))
            {
                switch (type)
                {
                    case MVMIH.Type.X:
                        return PhongMaterials.Red;
                    case MVMIH.Type.Y:
                        return PhongMaterials.Green;
                    case MVMIH.Type.Z:
                        return PhongMaterials.Blue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
