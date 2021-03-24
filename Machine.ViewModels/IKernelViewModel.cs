using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels
{
    public interface IKernelViewModel
    {
        IList<ElementViewModel> Machines { get; }
        ElementViewModel Selected { get; set; }
    }
}
