using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Machine.Data.MachineElements
{
    [Table("ToolholderElement")]
    public class ToolholderElement : MachineElement
    {
        public int ToolHolderId { get; set; }
        public ToolHolderType ToolHolderType { get; set; }
        public virtual Point Position { get; set; }
        public virtual Vector Direction { get; set; }
    }
}
