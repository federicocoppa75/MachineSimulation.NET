using Machine.Data.Base;
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

        private Matrix3D Convert(Matrix matrix)
        {
            return new Matrix3D(matrix.M11, matrix.M12, matrix.M13, 0.0,
                       matrix.M21, matrix.M22, matrix.M23, 0.0,
                       matrix.M31, matrix.M32, matrix.M33, 0.0,
                       matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ, 1.0);
        }
    }
}
