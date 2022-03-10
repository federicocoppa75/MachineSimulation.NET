using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages.Tooling
{
    internal class GetToolHolderSinkMessage
    {
        public int Sink { get; set; }
        public Action<IMachineElement> SetToolHolder { get; set; }
    }
}
