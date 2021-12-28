using Machine.Data.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages.Tooling
{
    public class SaveToolsMessage
    {
        public Action<IEnumerable<ITool>> GetTools { get; set; }
    }
}
