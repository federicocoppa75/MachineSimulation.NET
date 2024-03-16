using g3;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels.Extensions
{
    static class ToolHelper
    {
        static internal AxisAlignedBox3d GetBound(Vector3f position, float radius, float length, Orientation orientation, Vector3f direction)
        {
            if (orientation == Orientation.Any) return GetBound(position, radius, length, direction);

            var min = position;
            var max = min;

            switch (orientation)
            {
                case Orientation.XPos:
                    min -= new Vector3f(length, radius, radius);
                    max += new Vector3f(0.0, radius, radius);
                    break;
                case Orientation.XNeg:
                    min -= new Vector3f(0.0, radius, radius);
                    max += new Vector3f(length, radius, radius);
                    break;
                case Orientation.YPos:
                    min -= new Vector3f(radius, length, radius);
                    max += new Vector3f(radius, 0.0, radius);
                    break;
                case Orientation.YNeg:
                    min -= new Vector3f(radius, 0.0, radius);
                    max += new Vector3f(radius, length, radius);
                    break;
                case Orientation.ZPos:
                    min -= new Vector3f(radius, radius, length);
                    max += new Vector3f(radius, radius, 0.0);
                    break;
                case Orientation.ZNeg:
                    min -= new Vector3f(radius, radius, 0.0);
                    max += new Vector3f(radius, radius, length);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new AxisAlignedBox3d(min, max);
        }

        static private AxisAlignedBox3d GetBound(Vector3f position, float radius, float length, Vector3f direction)
        {
            var v1 = new Vector3d(direction);
            var c =  new Vector3d(position - (direction * length / 2));
            Vector3d.MakePerpVectors(ref v1 , out Vector3d v2, out Vector3d v3);

            var bb = new Box3d(c, v1, v2, v3, new Vector3d(length / 2, radius / 2, radius / 2));

            return bb.ToAABB();
        }
    }
}
