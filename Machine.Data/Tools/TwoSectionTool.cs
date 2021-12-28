using Machine.Data.Interfaces.Tools;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine.Data.Tools
{
    [Table("TwoSectionTool")]
    public class TwoSectionTool : Tool, IWorkData, ITwoSectionTool
    {
        public double Diameter1 { get; set; }
        public double Length1 { get; set; }
        public double Diameter2 { get; set; }
        public double Length2 { get; set; }
        public double UsefulLength { get; set; }

        public override double GetTotalDiameter() => Math.Max(Diameter1, Diameter2);
        public override double GetTotalLength() => Length1 + Length2;

        public double GetUsefulLength() => UsefulLength;

        public double GetWorkLength() => GetTotalLength();

        public double GetWorkRadius() => Diameter2 / 2.0;
    }
}
