using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages
{
    public class MoveToolExecuteMessage
    {
        public int Sink { get; set; }
        public ElementViewModel Tool { get; set; }
    }
}
