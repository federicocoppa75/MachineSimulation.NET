using Machine.Data.Enums;

namespace Machine.Data.Interfaces.Tools
{
    public interface ITool
    {
        string Name { get; }
        string Description { get; }
        ToolLinkType ToolLinkType { get; }
        string ConeModelFile { get; }

        double GetTotalDiameter();
        double GetTotalLength();
    }
}
