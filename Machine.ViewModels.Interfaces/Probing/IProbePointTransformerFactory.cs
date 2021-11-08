using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Probing
{
    public interface IProbePointTransformerFactory
    {
        IProbePointTransformer GetTransformer(IMachineElement probeParent);
    }
}
