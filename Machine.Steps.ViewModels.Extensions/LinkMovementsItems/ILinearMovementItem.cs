using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    interface ILinearMovementItem : IMovementItem
    {
        double Value { get;  }
    }
}
