using HelixToolkit.Wpf.SharpDX;
using Machine.ViewModels.Interfaces.Insertions;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Machine._3D.Views.Converters
{
    class InjectedToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is IInjectedObject obj)
            {
                var builder = new MeshBuilder();
                var p1 = new Vector3((float)obj.Position.X, (float)obj.Position.Y, (float)obj.Position.Z);
                var d = new Vector3((float)obj.Direction.X, (float)obj.Direction.Y, (float)obj.Direction.Z);
                var p2 = p1 + d * 20;

                builder.AddCone(p2, p1, 4.0, true, 20);

                return builder.ToMesh();
            }
            else
            {
                return null;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
