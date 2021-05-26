using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages
{
    public class AutoSourceToolholderChangedMessage
    {
        public int ToolholderId { get; set; }
        public string ToolName { get; set; }
    }
}
