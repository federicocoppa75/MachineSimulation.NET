using Machine.Data.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Tools
{
    public class SubspindleEx : Subspindle, ISubspindleEx
    {
        public Tool Tool { get; set; }
        public override string ToolName 
        { 
            get => (Tool != null) ? Tool.Name : null;
            set { } 
        }

        public ITool GetTool() => Tool;
        public void SetTool(ITool tool) => Tool = tool as Tool;
    }
}
