using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Tools;
using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Helpers
{
    public class ToolToPanelTransformerFactory : IToolToPanelTransformerFactory
    {
        public IToolToPanelTransformer GetTransformer(IPanelElement panel, IEnumerable<IToolElement> tools) => new ToolToPanelTransformer(panel, tools);
    }
}
