using Machine.Data.Base;
using Machine.ViewModels.Interfaces.Bridge;
using Machine.ViewModels.Interfaces.MachineElements;
using MDIT = Machine.Data.Interfaces.Tools;

namespace Machine.ViewModels.MachineElements
{
    public class ToolViewModel : ElementViewModel, IToolElement, IToolDataProxy
    {
        public MDIT.ITool Tool { get; set; }

        public Color ConeColor { get; set; } = new Color() { A = 255 };

        public string ConeModelFile => (Tool != null) ? Tool.ConeModelFile : null;

        public double WorkRadius => (Tool is MDIT.IWorkData wd) ? wd.GetWorkRadius() : 0.0;

        public double WorkLength => (Tool is MDIT.IWorkData wd) ? wd.GetWorkLength() : 0.0;

        public double UsefulLength => (Tool is MDIT.IWorkData wd) ? wd.GetUsefulLength() : 0.0;

        public ToolViewModel() : base()
        {
        }
    }
}
