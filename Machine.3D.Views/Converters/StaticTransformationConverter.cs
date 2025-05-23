﻿using Machine.Data.Base;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Machine._3D.Views.Converters
{
    public class StaticTransformationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Matrix matrix)) return Matrix3D.Identity;

            return Convert(matrix);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        static public Matrix3D Convert(Matrix matrix)
        {
            return new Matrix3D(matrix.M11, matrix.M12, matrix.M13, 0.0,
                       matrix.M21, matrix.M22, matrix.M23, 0.0,
                       matrix.M31, matrix.M32, matrix.M33, 0.0,
                       matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ, 1.0);
        }

        static public Matrix3D Convert(Vector v) => Convert(v.X, v.Y, v.Z);

        static public Matrix3D Convert(Point p) => Convert(p.X, p.Y, p.Z);
 
        static public Matrix3D Convert(double offsetX, double offsetY, double offsetZ)
        {
            var m = Matrix3D.Identity;

            m.OffsetX = offsetX;
            m.OffsetY = offsetY;
            m.OffsetZ = offsetZ;

            return m;
        }

        static public Matrix Convert(Matrix3D matrix)
        {
            return new Matrix() { M11 = matrix.M11, M12 = matrix.M12, M13 = matrix.M13,
                                  M21 = matrix.M21, M22 = matrix.M22, M23 = matrix.M23,
                                  M31 = matrix.M31, M32 = matrix.M32, M33 = matrix.M33,
                                  OffsetX = matrix.OffsetX, OffsetY = matrix.OffsetY, OffsetZ = matrix.OffsetZ };
        }
    }
}
