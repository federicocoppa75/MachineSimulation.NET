using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    interface ILinksMovementsGroup
    {
        TimeSpan Duration { get; }
        int GroupId { get; }
        bool IsCompleted { get; }
        List<IMovementItem> Items { get; }
        int NotifyId { get; }
        DateTime Start { get; }

        void Add(ILinkViewModel link, double targetValue);
        void Add(ILinkViewModel link, double targetValue, ILinkMovementManager.ArcComponentData data);
        bool Progress(DateTime now);
    }
}