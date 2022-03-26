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
using HelixToolkit.Wpf.SharpDX;

namespace Machine._3D.Views.Converters
{
    internal class RotationHandleToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value != null) && (value is MVMIH.IRotationHandle rh))
            {
                var me = rh as IMachineElement;
                var parent = me.Parent;
                var eh = parent as MVMIH.IElementHandle;

                if (eh != null)
                {
                    var bb = new BoundingBox(new Vector3((float)eh.MinX, (float)eh.MinY, (float)eh.MinZ), new Vector3((float)eh.MaxX, (float)eh.MaxY, (float)eh.MaxZ));
                    var c = bb.Center();
                    var diameter = GetDiameter(bb, rh.Type);
                    var distance = GetDistance(bb, rh.Type);
                    var length = diameter / 12.0f;
                    var direction = GetDirection(rh.Type);
                    var p1 = c + direction * distance;
                    var p2 = c - direction * distance;
                    var p3 = c + direction * (distance + length);
                    var p4 = c - direction * (distance + length);
                    var builder = new MeshBuilder();

                    builder.AddCylinder(p1, p3, diameter / 2.0f, 32, false, false);
                    //builder.AddCylinder(p2, p4, diameter / 2.0f, 32, false, false);

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
                    result = (box.Height + box.Depth) / 2.0f;//Math.Min(box.Height, box.Depth);
                    break;
                case MVMIH.Type.Y:
                    result = (box.Width + box.Depth) / 2.0f;// Math.Min(box.Width, box.Depth);
                    break;
                case MVMIH.Type.Z:
                    result = (box.Width + box.Height) / 2.0f;//Math.Min(box.Width, box.Height);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
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

            return (result / 2.0f) * 1.2f;
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
