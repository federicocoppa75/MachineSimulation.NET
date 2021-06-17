using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements.Toolholder
{
    public class AutoSyncToolholderElementViewModel : AutoToolholderElementViewModel
    {
        public override ToolHolderType ToolHolderType => ToolHolderType.AutoSink;
        public override IMachineElement Parent
        {
            get => base.Parent;
            set
            {
                base.Parent = value;
                if (base.Parent != null) AttachActivator();
            }
        }
    }
}
