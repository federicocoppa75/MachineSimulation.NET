

using Machine.Data.Interfaces.Tools;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine.Data.Tools
{
    [Table("SimpleTool")]
    public class SimpleTool : Tool, IWorkData
    {
        public double Diameter { get; set; }
        public double Length { get; set; }
        public double UsefulLength { get; set; }

        public override double GetTotalDiameter() => Diameter;
        public override double GetTotalLength() => Length;

        public double GetUsefulLength() => UsefulLength;

        public double GetWorkLength() => GetTotalLength();

        public double GetWorkRadius() => Diameter / 2.0;
    }
}
