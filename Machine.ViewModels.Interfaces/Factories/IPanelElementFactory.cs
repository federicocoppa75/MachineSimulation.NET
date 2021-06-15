using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Factories
{
    public interface IPanelElementFactory
    {
        IPanelElement Create(double centerX, double centerY, double centerZ, double sizeX, double sizeY, double sizeZ);
    }
}
