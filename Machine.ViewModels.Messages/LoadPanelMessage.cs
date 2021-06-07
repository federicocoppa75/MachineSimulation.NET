using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages
{
    public struct LoadPanelMessage
    {
        public int PanelHolderId { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Action<bool> NotifyExecution { get; set; }
    }
}
