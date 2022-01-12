using System;
using System.Globalization;
using System.Windows.Data;
using MVMUI = Machine.ViewModels.UI;
using M3DGPI = Machine._3D.Geometry.Provider.Interfaces;
using HelixToolkit.Wpf.SharpDX;
using MVMIOC = Machine.ViewModels.Ioc;

namespace Machine._3D.Views.Converters
{
    class ModelNameToGeometryConverter : IValueConverter
    {
        private MVMUI.IOptionProvider _dataSource;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((value is string geometryName) && !string.IsNullOrEmpty(geometryName))
            {
                if (_dataSource == null) _dataSource = MVMIOC.SimpleIoc<MVMUI.IOptionProvider>.GetInstance();

                var provider = MVMIOC.SimpleIoc<M3DGPI.IStreamProvider>.GetInstance(_dataSource.ToString());

                if (provider != null)
                {
                    Geometry3D geometry = null;

                    try
                    {
                        var stream = provider.GetStream(geometryName);

                        if (stream != null)
                        {
                            using (stream)
                            {
                                var reader = new StLReader();
                                var objList = reader.Read(stream);

                                if (objList?.Count > 0)
                                {
                                    geometry = objList[0].Geometry;
                                    geometry.UpdateOctree();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (MVMIOC.SimpleIoc<MVMUI.IExceptionObserver>.TryGetInstance(out MVMUI.IExceptionObserver observer))
                        {
                            observer.NotifyException(e);
                        }
                    }

                    return geometry;
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
    }
}
