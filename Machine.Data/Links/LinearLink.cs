using System.ComponentModel.DataAnnotations.Schema;

namespace Machine.Data.Links
{
    [Table("LinearLink")]
    public class LinearLink : Link
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Pos { get; set; }
    }
}
