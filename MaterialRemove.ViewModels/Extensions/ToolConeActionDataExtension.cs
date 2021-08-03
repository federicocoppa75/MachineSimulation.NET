using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels.Extensions
{
    static class ToolConeActionDataExtension
    {
        static internal ToolConeApplication ToApplication(this ToolConeActionData toolConeActionData, int index = -1)
        {
            return new ToolConeApplication(new g3.Vector3f(toolConeActionData.X, toolConeActionData.Y, toolConeActionData.Z),
                                          toolConeActionData.MinRadius,
                                          toolConeActionData.MaxRadius,
                                          toolConeActionData.Length,
                                          toolConeActionData.Orientation,
                                          index);
        }
    }
}
