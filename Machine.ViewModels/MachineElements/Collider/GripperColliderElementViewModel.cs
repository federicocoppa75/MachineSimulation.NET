using Machine.Data.Enums;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Messages;
using Machine.ViewModels.UI.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements.Collider
{
    [MachineStruct("Gripper", 6)]
    public class GripperColliderElementViewModel : ColliderElementViewModel, IPanelHooker
    {
        private bool _isGripping;

        public override ColliderType Type => ColliderType.Gripper;
        public ILinkViewModel Link => GetFirstLinearLink();

        protected override void OnPneumaticLinkStateChanging(object sender, bool e)
        {
            if (e) EvaluatePanelCollision(sender as IPneumaticLinkViewModel);
        }

        protected override void OnPneumaticLinkStateChanged(object sender, bool e)
        {
            if (!e && _isGripping) EvaluateReleasePanel(sender as IPneumaticLinkViewModel);
        }

        private void EvaluateReleasePanel(IPneumaticLinkViewModel pneumaticLinkViewModel)
        {
            Messenger.Send(new GetPanelMessage()
            {
                SetPanel = (pvm) =>
                {
                    pvm.HookingHandle.Unhook(this);
                    _isGripping = false;
                }
            });
        }

        private void EvaluatePanelCollision(IPneumaticLinkViewModel link)
        {
            Messenger.Send(new GetPanelMessage()
            {
                SetPanel = (pvm) =>
                {
                    var th = GetInstance<IColliderHelperFactory>();
                    var ch = th.GetColliderHelper(this, pvm);

                    if (ch.Intersect(out double d) && IsGripping(link, d))
                    {
                        link.DynOnPos = d;
                        pvm.HookingHandle.Hook(this);
                        _isGripping = true;
                    }
                }
            });
        }

        private ILinearLinkViewModel GetFirstLinearLink()
        {
            IMachineElement p = this;

            while (p != null)
            {
                if (p.LinkToParent is ILinearLinkViewModel link) return link;
                p = p.Parent;
            }

            return null;
        }

        private bool IsGripping(IPneumaticLinkViewModel link, double intersectValue)
        {
            var result = false;

            if(((link.OffPos <= intersectValue) && (intersectValue < link.OnPos)) ||
                ((link.OffPos >= intersectValue) && (intersectValue > link.OnPos)))
            {
                result = true;
            }

            return result;
        }
    }
}
