using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine.Data.Tools
{
    [Table("DiskTool")]
    public class DiskTool : Tool
    {
        public double Diameter { get; set; }
        public double CuttingRadialThickness { get; set; }
        public double BodyThickness { get; set; }
        public double CuttingThickness { get; set; }
        public double RadialUsefulLength { get; set; }

        public override double GetTotalDiameter() => Diameter;
        public override double GetTotalLength() => Math.Max(CuttingThickness, BodyThickness);
    }
}
