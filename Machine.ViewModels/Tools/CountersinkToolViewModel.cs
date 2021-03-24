using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Tools
{
    public class CountersinkToolViewModel : ToolViewModel
    {
        public double Diameter1 { get; set; }
        public double Length1 { get; set; }
        public double Diameter2 { get; set; }
        public double Length2 { get; set; }
        public double Length3 { get; set; }
        public double UsefulLength { get; set; }
    }
}
