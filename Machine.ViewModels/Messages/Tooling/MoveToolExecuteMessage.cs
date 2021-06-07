using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages.Tooling
{
    public class MoveToolExecuteMessage
    {
        public int Sink { get; set; }
        public IMachineElement Tool { get; set; }
    }
}
