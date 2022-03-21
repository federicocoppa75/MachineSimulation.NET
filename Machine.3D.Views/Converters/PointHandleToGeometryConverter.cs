using HelixToolkit.Wpf.SharpDX;
using Machine.ViewModels.Interfaces.MachineElements;
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
    internal class PointHandleToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((value != null) && (value is MVMIH.IPositionHandle ph))
            {
                var me = ph as IMachineElement;
                var parent = me.Parent;
                var eh = parent as MVMIH.IElementHandle;

                if(eh != null)
                {
                    var bb = new BoundingBox(new Vector3((float)eh.MinX, (float)eh.MinY, (float)eh.MinZ), new Vector3((float)eh.MaxX, (float)eh.MaxY, (float)eh.MaxZ));
                    var c = bb.Center();
                    var diameter = GetDiameter(bb, ph.Type);
                    var distance = GetDistance(bb, ph.Type);
                    var length = diameter * 5.0f;
                    var direction = GetDirection(ph.Type);
                    var p1 = c + direction * distance;
                    var p2 = c - direction * distance;
                    var p3 = c + direction * (distance + length);
                    var p4 = c - direction * (distance + length);
                    var builder = new MeshBuilder();

                    builder.AddArrow(p1, p3, diameter);
                    builder.AddArrow(p2, p4, diameter);

                    return builder.ToMesh();
                }
                else
                {
                    return null;
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

        private float GetDiameter(BoundingBox box, MVMIH.Type type)
        {
            float result = 0.0f;

            switch (type)
            {
                case MVMIH.Type.X:
                    result = Math.Min(box.Height, box.Depth);
                    break;
                case MVMIH.Type.Y:
                    result = Math.Min(box.Width, box.Depth);
                    break;
                case MVMIH.Type.Z:
                    result = Math.Min(box.Width, box.Height);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result / 8.0f;
        }

        private float GetDistance(BoundingBox box, MVMIH.Type type)
        {
            float result = 0.0f;

            switch (type)
            {
                case MVMIH.Type.X:
                    result = box.Width;
                    break;
                case MVMIH.Type.Y:
                    result = box.Height;
                    break;
                case MVMIH.Type.Z:
                    result = box.Depth;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result / 2.0f;
        }

        private Vector3 GetDirection(MVMIH.Type type)
        {
            switch (type)
            {
                case MVMIH.Type.X:
                    return new Vector3(1.0f, 0.0f, 0.0f);
                case MVMIH.Type.Y:
                    return new Vector3(0.0f, 1.0f, 0.0f);
                case MVMIH.Type.Z:
                    return new Vector3(0.0f, 0.0f, 1.0f);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
