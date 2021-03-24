using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Tools
{
    public class PointedToolViewModel : ToolViewModel
    {
        public double Diameter { get; set; }
        public double StraightLength { get; set; }
        public double ConeHeight { get; set; }
        public double UsefulLength { get; set; }
    }
}
