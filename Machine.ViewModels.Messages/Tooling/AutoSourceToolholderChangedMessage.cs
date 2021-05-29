using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages.Tooling
{
    public class AutoSourceToolholderChangedMessage
    {
        public int ToolholderId { get; set; }
        public string ToolName { get; set; }
    }
}
