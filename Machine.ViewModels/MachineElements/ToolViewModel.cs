using Machine.Data.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;
using MDT = Machine.Data.Tools;

namespace Machine.ViewModels.MachineElements
{
    public class ToolViewModel : ElementViewModel
    {
        public MDT.Tool Tool { get; set; }

        public Color ConeColor { get; set; }

        public string ConeModelFile => (Tool != null) ? Tool.ConeModelFile : null;

        public ToolViewModel() : base()
        {
        }
    }
}
