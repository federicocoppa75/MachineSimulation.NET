using Machine.Data.Enums;
using Machine.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.ViewModels.MachineElements.Toolholder
{
    public class AutoSourceToolholderElementViewModel : AutoToolholderElementViewModel
    {
        public override ToolHolderType ToolHolderType => ToolHolderType.AutoSource;

        public AutoSourceToolholderElementViewModel() : base()
        {
        }

        protected override void OnLoadToolMessage(LoadToolMessage msg) => NotifyChildrenChangedAfterAction(() => base.OnLoadToolMessage(msg), msg.Tool.Name);

    }
}
