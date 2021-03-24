using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Tools
{
    public class SimpleToolViewModel : ToolViewModel
    {
        public double Diameter { get; set; }
        public double Length { get; set; }
        public double UsefulLength { get; set; }
    }
}
