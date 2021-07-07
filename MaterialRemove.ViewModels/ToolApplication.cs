using g3;
using Machine.ViewModels.Interfaces;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using System;

namespace MaterialRemove.ViewModels
{
    internal struct ToolApplication : BoundedImplicitFunction3d, IIndexed
    {
        public float Radius { get; }
        public float Length { get; }
        public Orientation Orientation { get; }
        public Vector3d Position { get; }
        public int Index { get; }

        public ToolApplication(Vector3d position, float radius, float length, Orientation orientation, int index)
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
    }
}
