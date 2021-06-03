using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Windows.Media.Media3D;

namespace Machine._3D.Views.Helpers
{
    abstract class BaseColliderHelper
    {
        public bool Intersect(out double distance)
        {
            var result = false;
            var matrix1 = GetColliderChainTransformation();
            var matrix2 = GetPanelChainTransformation();
            var colliderPoints = GetColliderPoints();
            var colliderDirection = GetColliderDirection();
            var panelCenter = GetPanelCenter();
            var panelSize = GetPanelSize();

            distance = 0.0;
            matrix1.Transform(colliderPoints);
            colliderDirection = matrix1.Transform(colliderDirection);
            panelCenter = matrix2.Transform(panelCenter);
            var panel = GetPanel(panelCenter, panelSize);

            foreach (var item in colliderPoints)
            {
                var ray = new Ray(item.ToVector3(), colliderDirection.ToVector3());

                if (panel.Intersects(ref ray, out Vector3 v))
                {
                    distance = GetDistance(v.ToPoint3D() - item, colliderDirection);
                    result = true;
                    break;
                }
            }

            return result;
        }

        private BoundingBox GetPanel(Point3D panelCenter, Size3D panelSize)
        {
            var v1 = new Vector3D(-panelSize.X / 2.0, -panelSize.Y / 2.0, -panelSize.Z / 2.0);
            var v2 = new Vector3D(panelSize.X / 2.0, panelSize.Y / 2.0, panelSize.Z / 2.0);
            var v = panelCenter - new Point3D();
            var min = v + v1;
            var max = v + v2;

            return new BoundingBox(min.ToVector3(), max.ToVector3());
        }
        protected abstract Matrix3D GetColliderChainTransformation();
        protected abstract Matrix3D GetPanelChainTransformation();
        protected abstract Point3D[] GetColliderPoints();
        protected abstract Vector3D GetColliderDirection();
        protected abstract Point3D GetPanelCenter();
        protected abstract Size3D GetPanelSize();
        protected double GetDistance(Vector3D v, Vector3D direction)
        {
            if (direction.X != 0.0) return v.X;
            else if (direction.Y != 0.0) return v.Y;
            else if (direction.Z != 0.0) return v.Z;
            else throw new ArgumentException();
        }

    }
}
