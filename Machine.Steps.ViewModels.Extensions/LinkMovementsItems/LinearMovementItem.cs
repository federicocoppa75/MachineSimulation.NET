using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    struct LinearMovementItem : ILinearMovementItem
    {
        public double Value { get; private set; }

        public ILinkViewModel Link { get; private set; }

        public double TargetValue { get; private set; }

        public static LinearMovementItem Create(ILinkViewModel link, double targetValue)
        {
            return new LinearMovementItem()
            {
                Link = link,
                Value = link.Value,
                TargetValue = targetValue
            };
        }

        public void SetValue(double k) => Link.Value = (TargetValue - Value) * k + Value;
    }
}
