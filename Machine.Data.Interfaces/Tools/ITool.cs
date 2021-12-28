using Machine.Data.Enums;

namespace Machine.Data.Interfaces.Tools
{
    public interface ITool
    {
        string Name { get; set; }
        string Description { get; set; }
        ToolLinkType ToolLinkType { get; set; }
        string ConeModelFile { get; set; }

        double GetTotalDiameter();
        double GetTotalLength();
    }
}
