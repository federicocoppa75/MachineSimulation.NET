using Machine.Data.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Tools
{
    public class Subspindle
    {
        public Point Position { get; set; }
        public Vector Direction { get; set; }
        public virtual string ToolName { get; set; }

    }
}
