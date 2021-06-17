using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Tools
{
    public interface IToolsObserver
    {
        void Register(IToolElement tool);
        void Unregister(IToolElement tool);
    }
}
