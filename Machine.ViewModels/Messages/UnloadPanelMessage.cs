using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages
{
    public class UnloadPanelMessage
    {
        public int PanelHolderId { get; set; }
        public Action<bool> NotifyExecution { get; set; }
    }
}
