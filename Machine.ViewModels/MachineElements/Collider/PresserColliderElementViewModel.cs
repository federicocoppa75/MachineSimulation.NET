using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements.Collider
{
    public class PresserColliderElementViewModel : ColliderElementViewModel
    {
        public override ColliderType Type => ColliderType.Presser;

        protected override void OnPneumaticLinkStateChanging(object sender, bool e)
        {
            throw new NotImplementedException();
        }
    }
}
