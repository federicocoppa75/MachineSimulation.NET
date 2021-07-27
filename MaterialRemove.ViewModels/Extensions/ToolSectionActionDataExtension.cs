using g3;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels.Extensions
{
    static class ToolSectionActionDataExtension
    {
        static internal ToolSectionApplication ToApplication(this ToolSectionActionData toolSectionActionData, int index = -1)
        {
            return new ToolSectionApplication(new Vector3f(toolSectionActionData.PX, toolSectionActionData.PY, toolSectionActionData.PZ),
                                              new Vector3f(toolSectionActionData.DX, toolSectionActionData.DY, toolSectionActionData.DZ),
                                              toolSectionActionData.FixBaseAx,
                                              toolSectionActionData.L,
                                              toolSectionActionData.W,
                                              toolSectionActionData.H,
                                              index);
        }

        static internal bool Intersect(this ToolSectionActionData toolSectionActionData, AxisAlignedBox3f box) => toolSectionActionData.ToApplication().Intersect(box);
    }
}
