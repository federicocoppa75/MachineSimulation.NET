
using Machine.Data.Interfaces.Tools;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine.Data.Tools
{
    [Table("DiskOnConeTool")]
    public class DiskOnConeTool : DiskTool, IWorkData
    {
        public double PostponemntLength { get; set; }
        public double PostponemntDiameter { get; set; }

        public override double GetTotalLength()
        {
            var bt = BodyThickness;
            var tl = base.GetTotalLength();

            return PostponemntLength + bt / 2.0 + tl / 2.0;
        }

        public override double GetWorkLength() => base.GetWorkLength() + PostponemntLength;
    }
}
