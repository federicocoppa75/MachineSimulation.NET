using Machine.Data.Interfaces.Tools;
using System;
using System.Collections.Generic;

namespace Machine.Views.Messages
{
    internal class ToolsRequestMessage
    {
        public Action<IEnumerable<ITool>> SetTools { get; set; }
    }
}
