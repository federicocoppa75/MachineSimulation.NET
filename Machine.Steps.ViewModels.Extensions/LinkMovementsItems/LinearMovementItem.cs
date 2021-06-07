using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    class LinearMovementItem : MovementItem
    {
        public double Value { get; private set; }

        public LinearMovementItem(ILinkViewModel link, double targetValue) : base(link, targetValue)
        {
            Value = link.Value;
        }

        public override void SetValue(double k) => Link.Value = (TargetValue - Value) * k + Value;
    }
}
