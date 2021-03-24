using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Toolings
{
    public class Tooling
    {
        public int ToolingID { get; set; }
        public string Name { get; set; }
        public string Machine { get; set; }
        public string Tools { get; set; }
        public virtual ICollection<ToolingUnit> Units { get; protected set; } = new List<ToolingUnit>();
    }
}
