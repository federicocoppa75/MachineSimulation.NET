using Machine.Steps.ViewModels.Interfaces.Models;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using MSME = MachineSteps.Models.Enums;

namespace Machine.Steps.ViewModels.Extensions.Models
{
    class TwoPositionLinkLazyAction : TwoPositionLinkAction, ILazyAction
    {
        public bool IsUpdated { get; private set; }

        public void Update()
        {
            var link = this.GetLink() as IPneumaticLinkViewModel;

            RequestedState = ((link != null) && link.State) ? MSME.TwoPositionLinkActionRequestedState.On : MSME.TwoPositionLinkActionRequestedState.Off;
            IsUpdated = true;
        }
    }
}
