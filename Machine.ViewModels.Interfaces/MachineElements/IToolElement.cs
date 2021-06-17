using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IToolElement : IMachineElement
    {
        double WorkRadius { get; }
        double WorkLength { get; }
        double UsefulLength { get; }
    }
}
