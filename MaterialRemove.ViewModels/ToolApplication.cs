using g3;
using Machine.ViewModels.Interfaces;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using MaterialRemove.ViewModels.Interfaces;
using System;

namespace MaterialRemove.ViewModels
{
    internal struct ToolApplication : BoundedImplicitFunction3d, IIndexed, IIntersector
    {
        public float Radius { get; }
        public float Length { get; }
        public Orientation Orientation { get; }
        public Vector3f Position { get; }
        public int Index { get; }

        public ToolApplication(Vector3f position, float radius, float length, Orientation orientation, int index)
        {
            Position = position;
            Radius = radius;
            Length = length;
            Orientation = orientation;
            Index = index;
        }

        #region BoundedImplicitFunction3d
        public AxisAlignedBox3d Bounds() => this.GetBound();

        public double Value(ref Vector3d pt)
        {
            var n = this.GetDirection() * -1.0;
            var v = pt - Position;
            var d = n.Dot(v);

            if((d < 0.0) || (d > Length))
            {
                return this.GetBound().SignedDistance(pt);
            }
            else
            {
                var orthoDist = Math.Sqrt(v.LengthSquared - d * d);

                return orthoDist - Radius;
            }

            throw new NotImplementedException();
        }
        #endregion

        #region IIntersector
        public bool Intersect(IPanel panel)
        {
            var panelBox = new AxisAlignedBox3d(new Vector3d(), panel.SizeX / 2.0, panel.SizeY / 2.0, panel.SizeZ / 2.0);
            var toolBox = this.GetBound();

            return panelBox.Intersects(toolBox);
        }

        public bool Intersect(IPanelSection section)
        {
            var sectionBox = section.GetBound();
            var toolBox = this.GetBound();

            return sectionBox.Intersects(toolBox);
        }

        public bool Intersect(ISectionFace face)
        {
            var toolBox = this.GetBound();
            var faceBox = face.GetBound();

            return faceBox.Intersects(toolBox);
        }
        #endregion
    }
}
