using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Probing
{
    public interface IProbe
    {
        int ProbeId { get; }
        double X { get; }
        double Y { get; }
        double Z { get; }
    }
}
