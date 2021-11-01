using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Probing
{
    public interface IProbePoint : IProbe
    {
        double RelativeX { get; }
        double RelativeY { get; }
        double RelativeZ { get; }
    }
}
