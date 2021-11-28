using Machine.Data.Enums;
using Machine.ViewModels.Messages.Tooling;
using Machine.ViewModels.UI.Attributes;

namespace Machine.ViewModels.MachineElements.Toolholder
{
    [MachineStruct("Tool holder (auto source)", 4)]
    public class AutoSourceToolholderElementViewModel : AutoToolholderElementViewModel
    {
        public override ToolHolderType ToolHolderType => ToolHolderType.AutoSource;

        public AutoSourceToolholderElementViewModel() : base()
        {
        }

        protected override void OnLoadToolMessage(LoadToolMessage msg) => NotifyChildrenChangedAfterAction(() => base.OnLoadToolMessage(msg), msg.Tool.Name);

        protected override void OnAngularTransmissionLoadMessage(AngularTransmissionLoadMessage msg) => NotifyChildrenChangedAfterAction(() => base.OnAngularTransmissionLoadMessage(msg), msg.AngularTransmission.Name);
    }
}
