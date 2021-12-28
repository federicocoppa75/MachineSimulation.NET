
using Machine.Data.Interfaces.Tools;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine.Data.Tools
{
    [Table("PointedTool")]
    public class PointedTool : Tool, IWorkData, IPointedTool
    {
        public double Diameter { get; set; }
        public double StraightLength { get; set; }
        public double ConeHeight { get; set; }
        public double UsefulLength { get; set; }

        public override double GetTotalDiameter() => Diameter;
        public override double GetTotalLength() => StraightLength + ConeHeight;

        public double GetUsefulLength() => UsefulLength;

        public double GetWorkLength() => GetTotalLength();

        public double GetWorkRadius() => Diameter / 2.0;
    }
}
