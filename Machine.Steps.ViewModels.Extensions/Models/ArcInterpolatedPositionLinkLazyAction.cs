using Machine.Steps.ViewModels.Interfaces.Models;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.Models
{
    class ArcInterpolatedPositionLinkLazyAction : ArcInterpolatedPositionLinkAction, ILazyAction
    {
        public bool IsUpdated { get; private set; }

        public void Update()
        {
            foreach (var item in Components)
            {
                item.TargetCoordinate = this.GetLink(item.LinkId).Value;
            }

            IsUpdated = true;
        }
    }
}
