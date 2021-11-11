using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Machine._3D.Views.Converters
{
    class SizeToLineConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if((values != null) && (values.Count() == 3) && (values[0] is double x) && (values[1] is double y) && (values[2] is double z))
            {
                var builder = new LineBuilder();
                var p0 = new SharpDX.Vector3();
                var p1 = new SharpDX.Vector3(0.0f, 0.0f, (float)(z / 2.0));
                var p2 = new SharpDX.Vector3(0.0f, (float)y, (float)(z / 2.0));
                var p3 = new SharpDX.Vector3((float)x, (float)y, (float)(z / 2.0));
                var p4 = new SharpDX.Vector3((float)x, (float)y, (float)z);

                builder.AddLine(p0, p1);
                builder.AddLine(p1, p2);
                builder.AddLine(p2, p3);
                builder.AddLine(p3, p4);

                return builder.ToLineGeometry3D();
            }
            else
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
