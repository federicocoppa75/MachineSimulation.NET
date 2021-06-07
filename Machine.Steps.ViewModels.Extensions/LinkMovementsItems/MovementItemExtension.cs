using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    static class MovementItemExtension
    {
        public static void SetTargetValue(this IMovementItem item) => item.Link.Value = item.TargetValue;
    }
}
