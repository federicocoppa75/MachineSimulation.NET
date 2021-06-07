using MDIT = Machine.Data.Interfaces.Tools;

namespace Machine.ViewModels.Messages.Tooling
{
    public struct LoadToolMessage
    {
        public int ToolHolder { get; set; }
        public MDIT.ITool Tool { get; set; }
    }
}
