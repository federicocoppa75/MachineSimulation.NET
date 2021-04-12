using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Tools
{
    public class SubspindleEx : Subspindle
    {
        public Tool Tool { get; set; }
        public override string ToolName 
        { 
            get => Tool.Name; 
            set => Tool.Name = value; 
        }
    }
}
