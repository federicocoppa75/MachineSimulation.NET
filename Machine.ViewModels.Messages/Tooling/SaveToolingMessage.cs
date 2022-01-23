using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages.Tooling
{
    public class SaveToolingMessage
    {
        public Action<int, string> AddToolUnit { get; set; }
    }
}
