﻿using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MDB = Machine.Data.Base;

namespace Machine._3D.Views.Converters
{
    internal class TwoPointToLineConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values != null) && (values.Count() == 2) && (values[0] is MDB.Point p1) && (values[1] is MDB.Point p2))
            {
                var builder = new LineBuilder();
                var v1 = new SharpDX.Vector3((float)p1.X, (float)p1.Y, (float)p1.Z);
                var v2 = new SharpDX.Vector3((float)p2.X, (float)p2.Y, (float)p2.Z);
                var d = v2 - v1;
                var n = d.Normalized();
                var len = d.Length();

                if((parameter != null) && (len <= 10.0))
                {
                    v2 += n * 6.0f;
                    v1 -= n * 6.0f;
                }

                builder.AddLine(v1, v2);

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
