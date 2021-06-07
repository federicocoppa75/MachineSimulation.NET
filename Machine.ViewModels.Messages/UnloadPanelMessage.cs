using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages
{
    public struct UnloadPanelMessage
    {
        public int PanelHolderId { get; set; }
        public Action<bool> NotifyExecution { get; set; }
    }
}
