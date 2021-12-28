namespace Machine.Data.Interfaces.Tools
{
    public interface IDiskTool : ITool
    {
        double BodyThickness { get; set; }
        double CuttingRadialThickness { get; set; }
        double CuttingThickness { get; set; }
        double Diameter { get; set; }
        double RadialUsefulLength { get; set; }
    }
}