using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IAngularTransmission : IMachineElement
    {
        string BodyModelFile { get; }
    }
}
