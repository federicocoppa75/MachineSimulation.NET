using System;
using System.Collections.Generic;
using System.Text;
using MDT = Machine.Data.Tools;

namespace Machine.ViewModels.Messages.Tooling
{
    public class LoadToolMessage
    {
        public int ToolHolder { get; set; }
        public MDT.Tool Tool { get; set; }
    }
}
