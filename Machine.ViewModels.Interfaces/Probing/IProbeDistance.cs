using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Probing
{
    public interface IProbeDistance : IProbe
    {
        IProbePoint Master { get; set; }
        IProbePoint Slave { get; set; }

        void Detach();
    }
}
