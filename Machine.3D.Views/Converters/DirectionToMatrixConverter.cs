using Machine.Data.Base;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Machine._3D.Views.Converters
{
    class DirectionToMatrixConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Vector v)
            {
                return Convert(v);
            }
            else
            {
                return Matrix3D.Identity;
            }
        }

        public static Matrix3D Convert(Vector v)
        {
            var t = new Vector() { X = 0.0, Y = 0.0, Z = -1.0 };
            var s = (v.X * t.X) + (v.Y * t.Y) + (v.Z * t.Z);

            if (s == 1.0)
            {
                return Matrix3D.Identity;
            }
            else if (s == -1.0)
            {
                return new Matrix3D() { M11 = -1.0, M22 = -1.0, M33 = -1.0, M44 = 1.0 };
            }
            else
            {
                return CreateRotatioMatrix(t, v);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static Matrix3D CreateRotatioMatrix(Vector start, Vector target)
        {
            var v1 = new Vector3D(start.X, start.Y, start.Z);
            var v2 = new Vector3D(target.X, target.Y, target.Z);
            var n = Vector3D.CrossProduct(v1, v2);
            var a = Vector3D.AngleBetween(v1, v2);

            n.Normalize();

            var at = new AxisAngleRotation3D() { Axis = n, Angle = a };
            var rt = new RotateTransform3D(at);

            return rt.Value;
        }
    }
}
