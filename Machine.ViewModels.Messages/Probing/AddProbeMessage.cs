using Machine.ViewModels.Interfaces.Probing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages.Probing
{
    public class AddProbeMessage
    {
        public IProbe Probe { get; set; }
    }
}
