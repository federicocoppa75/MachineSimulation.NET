

using System.ComponentModel.DataAnnotations.Schema;

namespace Machine.Data.Tools
{
    [Table("SimpleTool")]
    public class SimpleTool : Tool
    {
        public double Diameter { get; set; }
        public double Length { get; set; }
        public double UsefulLength { get; set; }

        public override double GetTotalDiameter() => Diameter;
        public override double GetTotalLength() => Length;
    }
}
