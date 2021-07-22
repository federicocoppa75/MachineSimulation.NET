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
        static internal AxisAlignedBox3d GetBound(Vector3f position, float radius, float length, Orientation orientation)
        {
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
    }
}
