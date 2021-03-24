

using Machine.Data.Enums;

namespace Machine.Data.Tools
{
    public abstract class Tool
    {
        public int ToolID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ToolLinkType ToolLinkType { get; set; }
        public string ConeModelFile { get; set; }

        public abstract double GetTotalDiameter();
        public abstract double GetTotalLength();
    }
}
