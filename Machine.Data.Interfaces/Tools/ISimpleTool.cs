namespace Machine.Data.Interfaces.Tools
{
    public interface ISimpleTool : ITool
    {
        double Diameter { get; set; }
        double Length { get; set; }
        double UsefulLength { get; set; }
    }
}