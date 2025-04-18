﻿using g3;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels.Extensions
{
    static class ToolActionDataExtension
    {
        static internal ToolApplication ToApplication(this ToolActionData toolActionData, int index = -1)
        {
            return new ToolApplication(new Vector3f(toolActionData.X, toolActionData.Y, toolActionData.Z),
                                        toolActionData.Radius,
                                        toolActionData.Length,
                                        toolActionData.Orientation,
                                        index,
                                        new Vector3f(toolActionData.DX, toolActionData.DY, toolActionData.DZ));
        }
    }
}
