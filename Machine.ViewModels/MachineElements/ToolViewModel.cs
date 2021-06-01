using Machine.Data.MachineElements;
using MDIT = Machine.Data.Interfaces.Tools;

namespace Machine.ViewModels.MachineElements
{
    public class ToolViewModel : ElementViewModel
    {
        public MDIT.ITool Tool { get; set; }

        public Color ConeColor { get; set; }

        public string ConeModelFile => (Tool != null) ? Tool.ConeModelFile : null;

        public ToolViewModel() : base()
        {
        }
    }
}
