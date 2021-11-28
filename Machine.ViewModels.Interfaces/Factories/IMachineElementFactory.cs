using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Factories
{
    public interface IMachineElementFactory
    {
        int Index { get; }
        string Label { get; }
        bool IsRoot { get; }

        IMachineElement Create();
    }
}
