using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Machine.Data.MachineElements
{
    [Table("RootElement")]
    public class RootElement : MachineElement
    {
        public string AssemblyName { get; set; }
        public RootType RootType { get; set; }
    }
}
