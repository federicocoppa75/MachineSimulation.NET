using Machine.ViewModels.Interfaces.Handles;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Factories
{
    public interface IHandleFactory
    {
        IElementHandle Create(IMachineElement element);
    }
}
