using System;
using System.Collections.Generic;
using System.Text;
using MDIT = Machine.Data.Interfaces.Tools;

namespace Machine.ViewModels.Interfaces.Bridge
{
    public interface IAngularTransmissionProxy
    {
        MDIT.IAngularTransmission Tool { get; }
    }
}
