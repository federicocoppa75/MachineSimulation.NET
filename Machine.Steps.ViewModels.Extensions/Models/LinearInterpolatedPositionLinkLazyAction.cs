using Machine.Steps.ViewModels.Interfaces.Models;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.Models
{
    class LinearInterpolatedPositionLinkLazyAction : LinearInterpolatedPositionLinkAction, ILazyAction
    {
        public bool IsUpdated { get; private set; }

        public void Update()
        {
            foreach (var pos in Positions)
            {
                var link = this.GetLink(pos.LinkId);
                if(link != null) pos.RequestPosition = link.Value;
            }

            IsUpdated = true;
        }
    }
}
