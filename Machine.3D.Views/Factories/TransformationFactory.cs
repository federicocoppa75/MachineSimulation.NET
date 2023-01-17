using Machine.Data.Base;
using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.Interfaces.Links;
using System;
using System.Windows.Media.Media3D;

namespace Machine._3D.Views.Factories
{
    class TransformationFactory : IViewTransformationFactory
    {

        public object CreateTranslation(Matrix matrix, ILinkViewModel link)
        {
            if (link == null)
            {
                return CreateTransformation3D(matrix);
            }
            else
            {
                return CreateTranslation3D(matrix, link);
            }
        }

        public Transform3D CreateTranslation3D(Matrix matrix, ILinkViewModel link)
        {
            var tg = new Transform3DGroup();
            var st = CreateTransformation3D(matrix);
            //var tt = new TranslateTransform3D();

            tg.Children.Add(st);
            //tg.Children.Add(tt);

            //Action<double> action = (d) => tt.OffsetX = d;
            //link.ValueChanged += (s, e) => action(e); 

            switch (link.Type)
            {
                case Data.Enums.LinkType.Linear:
                    tg.Children.Add(CreateLinearTRandormation3D(link));
                    break;
                case Data.Enums.LinkType.Rotary:
                    tg.Children.Add(CreateRotaryTransformation3D(link, st.Transform(new Point3D())));
                    break;
                default:
                    throw new ArgumentException();
            }


            return tg;
        }


        private Transform3D CreateTransformation3D(Matrix matrix)
        {
            var m3d = new Matrix3D(matrix.M11, matrix.M12, matrix.M13, 0.0,
                       matrix.M21, matrix.M22, matrix.M23, 0.0,
                       matrix.M31, matrix.M32, matrix.M33, 0.0,
                       matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ, 1.0);
            return new MatrixTransform3D(m3d);
        }

        /// <summary>
        /// Restituisce la trasformazione dell'asse lineare (inteso come asse non rotativo, lineare o pneumatico)
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private Transform3D CreateLinearTRandormation3D(ILinkViewModel link)
        {
            Action<double> action = null;
            var tt = new TranslateTransform3D();
            var offset = (link is ILinearLinkViewModel linearLink) ? linearLink.Pos : 0.0;
            var direction = link.Direction;

            switch (direction)
            {
                case LinkDirection.X:
                    tt.OffsetX = link.Value - offset;
                    action = (d) => tt.OffsetX = d - offset;
                    break;
                case LinkDirection.Y:
                    tt.OffsetY = link.Value - offset;
                    action = (d) => tt.OffsetY = d - offset;
                    break;
                case LinkDirection.Z:
                    tt.OffsetZ = link.Value - offset;
                    action = (d) => tt.OffsetZ = d - offset;
                    break;
                default:
                    throw new ArgumentException("Invalid traslation direction!");
            }


            link.ValueChanged += (s, e) => action(e);

            return tt;
        }

        private Transform3D CreateRotaryTransformation3D(ILinkViewModel link, Point3D rotCenter)
        {
            var rotVector = GetRotationDirection(link.Direction);
            var ar = new AxisAngleRotation3D(rotVector, 0.0);
            var tr = new RotateTransform3D(ar, rotCenter);

            if (link is ILinearLinkViewModel linearLink)
            {
                var offset = linearLink.Pos;

                link.ValueChanged += (s, e) => ar.Angle = e - offset;
            }
            else
            {
                link.ValueChanged += (s, e) => ar.Angle = e;
            }

            return tr;
        }

        private static Vector3D GetRotationDirection(LinkDirection direction)
        {
            Vector3D vector;
            switch (direction)
            {
                case LinkDirection.X: vector = new Vector3D(1.0, 0.0, 0.0); break;
                case LinkDirection.Y: vector = new Vector3D(0.0, 1.0, 0.0); break;
                case LinkDirection.Z: vector = new Vector3D(0.0, 0.0, 1.0); break;
                default: throw new ArgumentException("Invalid rotation direction!");
            }

            return vector;
        }
    }
}
