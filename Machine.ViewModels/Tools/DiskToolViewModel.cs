using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Tools
{
    public class DiskToolViewModel : ToolViewModel
    {
        public double Diameter { get; set; }
        public double CuttingRadialThickness { get; set; }
        public double BodyThickness { get; set; }
        public double CuttingThickness { get; set; }
        public double RadialUsefulLength { get; set; }
    }
}
