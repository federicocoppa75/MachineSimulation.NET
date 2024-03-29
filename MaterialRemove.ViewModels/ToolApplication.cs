﻿using g3;
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
        public Vector3f Direction { get; }

        public ToolApplication(Vector3f position, float radius, float length, Orientation orientation, int index, Vector3f direction)
        {
            Position = position;
            Radius = radius;
            Length = length;
            Orientation = orientation;
            Index = index;
            Direction = direction;
        }

        #region BoundedImplicitFunction3d
        public AxisAlignedBox3d Bounds() => this.GetBound();

        public double Value(ref Vector3d pt)
        {
            var n = this.GetDirection() * -1.0;
            var v = pt - Position;
            var d = n.Dot(v);
            var orthoDist = Math.Sqrt(v.LengthSquared - d * d) - Radius;

            if ((d < 0.0) || (d > Length))
            {
                var parDist = (d < 0.0) ? (-d) : (d - Length);

                if (orthoDist > 0.0)
                {
                    return Math.Sqrt(Math.Pow(parDist, 2) + Math.Pow(orthoDist, 2));
                }
                else
                {
                    return parDist;
                }
            }
            else
            {
                return orthoDist;
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
