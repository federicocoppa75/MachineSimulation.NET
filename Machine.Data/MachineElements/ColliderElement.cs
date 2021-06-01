using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Machine.Data.MachineElements
{
    [Table("ColliderElement")]
    public class ColliderElement : MachineElement
    {
        public ColliderType Type { get; set; }
        public double Radius { get; set; }
        public virtual ICollection<Point> Points { get; set; } = new List<Point>();
    }
}
