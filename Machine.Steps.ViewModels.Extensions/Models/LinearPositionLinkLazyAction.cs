using Machine.Steps.ViewModels.Interfaces.Models;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.Models
{
    class LinearPositionLinkLazyAction : LinearPositionLinkAction, ILazyAction
    {
        public bool IsUpdated { get; private set; }

        public void Update()
        {
            RequestedPosition = this.GetLink().Value;
            IsUpdated = true;
        }
    }
}
