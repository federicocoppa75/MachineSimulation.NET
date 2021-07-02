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
    class InsertedToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is IInsertedObject insObj)
            {
                var builder = new MeshBuilder();
                var p1 = new Vector3((float)insObj.Position.X, (float)insObj.Position.Y, (float)insObj.Position.Z);
                var d = new Vector3((float)insObj.Direction.X, (float)insObj.Direction.Y, (float)insObj.Direction.Z);
                var p2 = p1 + d * (float)(insObj.Length);

                builder.AddCylinder(p1, p2, insObj.Diameter / 2.0);

                return builder.ToMesh();
            }
            else if(value is IInjectedObject injObj)
            {
                var builder = new MeshBuilder();
                var p1 = new Vector3((float)injObj.Position.X, (float)injObj.Position.Y, (float)injObj.Position.Z);
                var d = new Vector3((float)injObj.Direction.X, (float)injObj.Direction.Y, (float)injObj.Direction.Z);
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
