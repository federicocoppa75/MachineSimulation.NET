using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements.Toolholder
{
    public class AutoSyncToolholderElementViewModel : AutoToolholderElementViewModel
    {
        public override ToolHolderType ToolHolderType => ToolHolderType.AutoSink;
    }
}
