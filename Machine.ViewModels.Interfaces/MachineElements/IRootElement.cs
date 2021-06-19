using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IRootElement : IMachineElement
    {
        string AssemblyName { get; set; }
        RootType RootType { get; set; }
    }
}
