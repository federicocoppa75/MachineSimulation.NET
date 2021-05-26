using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements.Toolholder
{
    public class StaticToolholderElementViewModel : ToolholderElementViewModel
    {
        public override ToolHolderType ToolHolderType => ToolHolderType.Static;

        public StaticToolholderElementViewModel() : base()
        {
        }
    }
}
