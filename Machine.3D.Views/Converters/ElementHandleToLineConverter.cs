using HelixToolkit.Wpf.SharpDX;
using SharpDX;
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
    internal class ElementHandleToLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((value is MVMIH.IElementHandle eh) && (parameter is string str) && int.TryParse(str, out int idx))
            {
                GetPoints(eh, idx, out Vector3 p0, out Vector3 p1);

                var builder = new LineBuilder();

                builder.AddLine(p0, p1);

                return builder.ToLineGeometry3D();
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

        private void GetPoints(MVMIH.IElementHandle eh, int idx, out Vector3 p0, out Vector3 p1)
        {
            switch (idx)
            {
                case 1:
                    p0 = new Vector3((float)eh.MinX, (float)eh.MinY, (float)eh.MinZ);
                    p1 = new Vector3((float)eh.MaxX, (float)eh.MinY, (float)eh.MinZ);
                    break;
                case 2:
                    p0 = new Vector3((float)eh.MaxX, (float)eh.MinY, (float)eh.MinZ);
                    p1 = new Vector3((float)eh.MaxX, (float)eh.MaxY, (float)eh.MinZ);
                    break;
                case 3:
                    p0 = new Vector3((float)eh.MaxX, (float)eh.MaxY, (float)eh.MinZ);
                    p1 = new Vector3((float)eh.MinX, (float)eh.MaxY, (float)eh.MinZ);
                    break;
                case 4:
                    p0 = new Vector3((float)eh.MinX, (float)eh.MaxY, (float)eh.MinZ);
                    p1 = new Vector3((float)eh.MinX, (float)eh.MinY, (float)eh.MinZ);
                    break;
                case 5:
                    p0 = new Vector3((float)eh.MinX, (float)eh.MinY, (float)eh.MaxZ);
                    p1 = new Vector3((float)eh.MaxX, (float)eh.MinY, (float)eh.MaxZ);
                    break;
                case 6:
                    p0 = new Vector3((float)eh.MaxX, (float)eh.MinY, (float)eh.MaxZ);
                    p1 = new Vector3((float)eh.MaxX, (float)eh.MaxY, (float)eh.MaxZ);
                    break;
                case 7:
                    p0 = new Vector3((float)eh.MaxX, (float)eh.MaxY, (float)eh.MaxZ);
                    p1 = new Vector3((float)eh.MinX, (float)eh.MaxY, (float)eh.MaxZ);
                    break;
                case 8:
                    p0 = new Vector3((float)eh.MinX, (float)eh.MaxY, (float)eh.MaxZ);
                    p1 = new Vector3((float)eh.MinX, (float)eh.MinY, (float)eh.MaxZ);
                    break;
                case 9:
                    p0 = new Vector3((float)eh.MinX, (float)eh.MinY, (float)eh.MinZ);
                    p1 = new Vector3((float)eh.MinX, (float)eh.MinY, (float)eh.MaxZ);
                    break;
                case 10:
                    p0 = new Vector3((float)eh.MaxX, (float)eh.MinY, (float)eh.MinZ);
                    p1 = new Vector3((float)eh.MaxX, (float)eh.MinY, (float)eh.MaxZ); 
                    break;
                case 11:
                    p0 = new Vector3((float)eh.MaxX, (float)eh.MaxY, (float)eh.MinZ);
                    p1 = new Vector3((float)eh.MaxX, (float)eh.MaxY, (float)eh.MaxZ);
                    break;
                case 12:
                    p0 = new Vector3((float)eh.MinX, (float)eh.MaxY, (float)eh.MinZ);
                    p1 = new Vector3((float)eh.MinX, (float)eh.MaxY, (float)eh.MaxZ);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
