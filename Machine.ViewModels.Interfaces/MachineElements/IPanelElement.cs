using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IPanelElement : IMachineElement
    {
        double SizeX { get; set; }
        double SizeY { get; set; }
        double SizeZ { get; set; }
        double CenterX { get; set; }
        double CenterY { get; set; }
        double CenterZ { get; set; }
    }
}
