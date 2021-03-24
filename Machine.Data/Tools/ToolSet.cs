using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Tools
{
    public class ToolSet
    {
        public int ToolSetID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Tool> Tools { get; protected set; } = new List<Tool>();
    }
}
