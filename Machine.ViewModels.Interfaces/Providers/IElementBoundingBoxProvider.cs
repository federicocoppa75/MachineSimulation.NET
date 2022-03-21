using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Providers
{
    public interface IElementBoundingBoxProvider
    {
        bool GetBoundingBox(IMachineElement element, out double minX, out double minY, out double minZ, out double maxX, out double maxY, out double maxZ);
    }
}
