using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Machine.Data.MachineElements
{
    [Table("InjectorElement")]
    public class InjectorElement : MachineElement
    {
        public int InserterId { get; set; }
        public virtual Vector Position { get; set; }
        public virtual Vector Direction { get; set; }
        public virtual Color InserterColor { get; set; }
    }
}
