

using Machine.ViewModels.Interfaces.Links;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    interface IArcMovementItem : IMovementItem
    {
        double Angle { get; set; }
        double CenterCoordinate { get; set; }
        double Radius { get; set; }
        double StartAngle { get; set; }
        ILinkMovementManager.ArcComponent ArcComponent { get; set; }
    }
}