using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    abstract class MovementItem
    {
        public ILinkViewModel Link { get; private set; }
        public double TargetValue { get; private set; }

        public MovementItem(ILinkViewModel link, double targetValue)
        {
            Link = link;
            TargetValue = targetValue;
        }

        public abstract void SetValue(double k);

        public void SetTargetValue() => Link.Value = TargetValue;
    }
}
