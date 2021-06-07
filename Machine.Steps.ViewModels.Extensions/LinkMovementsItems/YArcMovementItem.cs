using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    class YArcMovementItem : ArcMovementItem
    {
        public YArcMovementItem(ILinkViewModel link, double targetValue) : base(link, targetValue)
        {
        }

        public override void SetValue(double k)
        {
            double a = Normalize(StartAngle + Angle * k);

            Link.Value = CenterCoordinate + Math.Sin(a) * Radius;
        }
    }
}
