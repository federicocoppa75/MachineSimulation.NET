using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IATToolholder : IMachineElement
    {
        Point Position { get; set; }
        Vector Direction { get; set; }
    }
}
