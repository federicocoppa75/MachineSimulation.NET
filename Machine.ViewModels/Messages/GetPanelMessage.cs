using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages
{
    class GetPanelMessage
    {
        public Action<PanelViewModel> SetPanel { get; set; }
    }
}
