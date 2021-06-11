using g3;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels.Extensions
{
    static class ToolApplicationExtension
    {
        static internal AxisAlignedBox3d GetBound(this ToolApplication toolApplication)
        {
            return ToolHelper.GetBound(toolApplication.Position,
                                        toolApplication.Radius,
                                        toolApplication.Length,
                                        toolApplication.Orientation);
        }

        static internal Vector3d GetDirection(this ToolApplication toolApplication)
        {
            switch (toolApplication.Orientation)
            {
                case Orientation.XPos:
                    return new Vector3d(1.0, 0.0, 0.0);
                case Orientation.XNeg:
                    return new Vector3d(-1.0, 0.0, 0.0);
                case Orientation.YPos:
                    return new Vector3d(0.0, 1.0, 0.0);
                case Orientation.YNeg:
                    return new Vector3d(0.0, -1.0, 0.0);
                case Orientation.ZPos:
                    return new Vector3d(0.0, 0.0, 1.0);
                case Orientation.ZNeg:
                    return new Vector3d(0.0, 0.0, -1.0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
