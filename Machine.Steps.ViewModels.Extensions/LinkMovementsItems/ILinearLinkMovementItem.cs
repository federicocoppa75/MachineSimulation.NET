using Machine.ViewModels.Interfaces.Links;
using System;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    interface ILinearLinkMovementItem
    {
        TimeSpan Duration { get; }
        bool IsCompleted { get; set; }
        ILinkViewModel Link { get; }
        int NotifyId { get; }
        DateTime Start { get; }
        double TargetValue { get; }
        double Value { get; }

        bool Progress(DateTime now);
    }
}