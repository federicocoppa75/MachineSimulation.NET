using g3;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels.Extensions
{
    static class ToolConeApplicationExtension
    {
        static internal AxisAlignedBox3d GetBound(this ToolConeApplication toolConeApplication)
        {
            return ToolHelper.GetBound(toolConeApplication.Position,
                                        toolConeApplication.MaxRadius,
                                        toolConeApplication.Length,
                                        toolConeApplication.Orientation);
        }

        static internal Vector3d GetDirection(this ToolConeApplication toolConeApplication)
        {
            switch (toolConeApplication.Orientation)
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
