using MDT = Machine.Data.Tools;
using MDB = Machine.Data.Base;

namespace Machine.ViewModels.MachineElements
{
    public class ToolViewModel : ElementViewModel
    {
        public MDT.Tool Tool { get; set; }

        public MDB.Color ConeColor { get; set; }

        public string ConeModelFile => (Tool != null) ? Tool.ConeModelFile : null;

        public ToolViewModel() : base()
        {
        }
    }
}
