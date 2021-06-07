using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    class XArcMovementItem : ArcMovementItem
    {
        public XArcMovementItem(int linkId, double targetValue) : base(linkId, targetValue)
        {
        }

        public override void SetValue(double k)
        {
            double a = Normalize(StartAngle + Angle * k);

            ActualValue = CenterCoordinate + Math.Cos(a) * Radius;
        }
    }
}
