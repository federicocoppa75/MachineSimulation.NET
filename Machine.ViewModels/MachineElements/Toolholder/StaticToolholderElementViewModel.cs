using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements.Toolholder
{
    public class StaticToolholderElementViewModel : ToolholderElementViewModel
    {
        public override ToolHolderType ToolHolderType => ToolHolderType.Static;
        public override IMachineElement Parent 
        { 
            get => base.Parent;
            set
            {
                base.Parent = value;
                if (base.Parent != null) AttachActivator();
            }
        }

        public StaticToolholderElementViewModel() : base()
        {
        }


    }
}
