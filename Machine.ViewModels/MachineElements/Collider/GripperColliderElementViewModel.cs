using Machine.Data.Enums;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements.Collider
{
    public class GripperColliderElementViewModel : ColliderElementViewModel
    {
        public override ColliderType Type => ColliderType.Gripper;

        protected override void OnPneumaticLinkStateChanging(object sender, bool e)
        {
            if (e && Type == ColliderType.Gripper) EvaluatePanelGripperCollision(sender as IPneumaticLinkViewModel);
        }

        private void EvaluatePanelGripperCollision(IPneumaticLinkViewModel link)
        {
            Messenger.Send(new GetPanelMessage()
            {
                SetPanel = (pvm) =>
                {
                    var th = GetInstance<IColliderHelperFactory>();
                    var ch = th.GetColliderHelper(this, pvm);

                    if (ch.Intersect(out double d))
                    {
                        link.DynOnPos = d;
                    }
                }
            });
        }
    }
}
