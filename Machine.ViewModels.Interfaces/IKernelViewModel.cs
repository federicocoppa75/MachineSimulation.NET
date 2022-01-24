using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;

namespace Machine.ViewModels.Interfaces
{
    public interface IKernelViewModel
    {
        IList<IMachineElement> Machines { get; }
        IMachineElement Selected { get; set; }
        event EventHandler SelectedChanged;
        event EventHandler MachinesCollectionChanged;
    }
}
