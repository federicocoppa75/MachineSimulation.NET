using HelixToolkit.Wpf.SharpDX;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Machine._3D.Views.Converters
{
    class PanelToGemetryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is IPanelElement pvm)
            {
                var builder = new MeshBuilder();

                builder.AddBox(new SharpDX.Vector3(0.0f, 0.0f, 0.0f), pvm.SizeX, pvm.SizeY, pvm.SizeZ);

                return builder.ToMesh();
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
