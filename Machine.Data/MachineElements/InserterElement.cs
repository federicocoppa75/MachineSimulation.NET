using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Machine.Data.MachineElements
{
    [Table("InserterElement")]
    public class InserterElement : InjectorElement
    {
        public double Diameter { get; set; }
        public double Length { get; set; }
        public int LoaderLinkId { get; set; }
        public int DischargerLinkId { get; set; }
    }
}
