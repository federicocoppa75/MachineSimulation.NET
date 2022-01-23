using Machine.Data.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.Messages.ToolsEditor
{
    public class ToolSelectionChangedMessage
    {
        public ITool Tool { get; set; }
    }
}
