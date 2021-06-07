using Machine.ViewModels.Interfaces.Links;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    interface IMovementItem
    {
        ILinkViewModel Link { get; }
        double TargetValue { get; }
        void SetValue(double k);
    }
}