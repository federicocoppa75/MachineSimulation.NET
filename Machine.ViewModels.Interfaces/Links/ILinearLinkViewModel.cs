namespace Machine.ViewModels.Interfaces.Links
{
    public interface ILinearLinkViewModel : ILinkViewModel
    {
        double Max { get; set; }
        double Min { get; set; }
        double Pos { get; set; }
        bool Overflow { get; }
    }
}