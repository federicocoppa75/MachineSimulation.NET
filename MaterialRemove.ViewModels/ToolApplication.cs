﻿using g3;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels
{
    internal struct ToolApplication : BoundedImplicitFunction3d
    {
        public float Radius { get; }
        public float Length { get; }
        public Orientation Orientation { get; }
        public Vector3d Position { get; }

        public ToolApplication(Vector3d position, float radius, float length, Orientation orientation)
        {
            Position = position;
            Radius = radius;
            Length = length;
            Orientation = orientation;
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