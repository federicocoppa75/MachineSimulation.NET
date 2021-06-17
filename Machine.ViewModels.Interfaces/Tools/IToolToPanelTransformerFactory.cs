using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Tools
{
    public interface IToolToPanelTransformerFactory
    {
        IToolToPanelTransformer GetTransformer(IPanelElement panel, IEnumerable<IToolElement> tools);
    }
}
