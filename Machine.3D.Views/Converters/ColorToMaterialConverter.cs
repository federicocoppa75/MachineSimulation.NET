using HelixToolkit.Wpf.SharpDX;
using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Machine._3D.Views.Converters
{
    class ColorToMaterialConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Color color)
            {
                var material = new PhongMaterial();

                material.AmbientColor = new SharpDX.Color4(0.1f);
                material.DiffuseColor = new SharpDX.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
                material.SpecularColor = new SharpDX.Color4(0.0225f, 0.0225f, 0.0225f, 1.0f);
                material.SpecularShininess = 12.8f;

                return material;
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
