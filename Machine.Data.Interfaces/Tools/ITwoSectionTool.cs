namespace Machine.Data.Interfaces.Tools
{
    public interface ITwoSectionTool : ITool
    {
        double Diameter1 { get; set; }
        double Diameter2 { get; set; }
        double Length1 { get; set; }
        double Length2 { get; set; }
        double UsefulLength { get; set; }
    }
}