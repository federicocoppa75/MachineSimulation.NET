namespace Machine.Data.Interfaces.Tools
{
    public interface IPointedTool : ITool
    {
        double ConeHeight { get; set; }
        double Diameter { get; set; }
        double StraightLength { get; set; }
        double UsefulLength { get; set; }
    }
}