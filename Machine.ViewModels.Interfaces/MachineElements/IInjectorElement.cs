using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IInjectorElement : IMachineElement
    {
        int InserterId { get; set; }
        Point Position { get; set; }
        Vector Direction { get; set; }
        Color InserterColor { get; set; }
    }
}
