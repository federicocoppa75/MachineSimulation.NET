using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages
{
    public class MoveToolRequestMessage
    {
        public int Source { get; set; }
        public int Sink { get; set; }
    }
}
