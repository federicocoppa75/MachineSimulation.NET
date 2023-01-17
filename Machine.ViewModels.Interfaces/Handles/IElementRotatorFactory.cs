using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Handles
{
    public interface IElementRotatorFactory
    {
        IElementRotator Create(IMachineElement element);
    }
}
