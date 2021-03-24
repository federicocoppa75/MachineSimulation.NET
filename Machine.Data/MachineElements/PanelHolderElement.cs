using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Machine.Data.MachineElements
{
    [Table("PanelHolderElement")]
    public class PanelHolderElement : MachineElement
    {
        public int PanelHolderId { get; set; }
        public string PanelHolderName { get; set; }
        public virtual Vector Position { get; set; }
        public PanelLoadType Corner { get; set; }
    }
}
