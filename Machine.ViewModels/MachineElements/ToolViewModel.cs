using Machine.Data.Base;
using Machine.Data.Interfaces.Tools;
using Machine.ViewModels.Interfaces.MachineElements;
using MDIT = Machine.Data.Interfaces.Tools;

namespace Machine.ViewModels.MachineElements
{
    public class ToolViewModel : ElementViewModel, IToolElement
    {
        public MDIT.ITool Tool { get; set; }

        public Color ConeColor { get; set; }

        public string ConeModelFile => (Tool != null) ? Tool.ConeModelFile : null;

        public double WorkRadius => (Tool is IWorkData wd) ? wd.GetWorkRadius() : 0.0;

        public double WorkLength => (Tool is IWorkData wd) ? wd.GetWorkLength() : 0.0;

        public double UsefulLength => (Tool is IWorkData wd) ? wd.GetUsefulLength() : 0.0;

        public ToolViewModel() : base()
        {
        }
    }
}
