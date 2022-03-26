using HelixToolkit.Wpf.SharpDX;
using Machine._3D.Views.Converters;
using Machine.ViewModels.Handles;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Machine._3D.Views.Helpers
{
    internal class ElementRotator : IElementRotator
    {
        IMachineElement _element;
        Matrix3D _matrix;
        Point3D _center;

        public ElementRotator(IMachineElement element)
        {
            _element = element;
            _matrix = StaticTransformationConverter.Convert(element.Transformation);
            _center = GetCenter(element);
        }

        public void Rotate(double x, double y, double z, double angle)
        {
            var v = _matrix.Transform(new Vector3D(x, y, z));
            var c = _matrix.Transform(_center);
            var tg = new Transform3DGroup();
            var tm = new MatrixTransform3D(_matrix);
            var tr = new RotateTransform3D(new QuaternionRotation3D(new Quaternion(v, angle)), c);

            tg.Children.Add(tm);
            tg.Children.Add(tr);

            _element.Transformation = StaticTransformationConverter.Convert(tg.Value);
        }

        private Point3D GetCenter(IMachineElement element)
        {
            Machine.ViewModels.Ioc.SimpleIoc<IElementBoundingBoxProvider>
                .GetInstance()
                .GetBoundingBox(element, 
                                out double minX, 
                                out double minY, 
                                out double minZ, 
                                out double maxX, 
                                out double maxY, 
                                out double maxZ);

            var bb = new SharpDX.BoundingBox(new SharpDX.Vector3((float)minX, (float)minY, (float)minZ), new SharpDX.Vector3((float)maxX, (float)maxY, (float)maxZ));

            return bb.Center.ToPoint3D();
        }
    }
}
