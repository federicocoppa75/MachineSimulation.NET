using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    abstract class MovementItem
    {
        public int LinkId { get; private set; }

        public double TargetValue { get; private set; }

        public double ActualValue { get; protected set; }

        public MovementItem(int linkId, double targetValue)
        {
            LinkId = linkId;
            TargetValue = targetValue;
        }

        public abstract void SetValue(double k);

        public void SetTargetValue() => ActualValue = TargetValue;
    }
}
