using MaterialRemove.Interfaces;
using MaterialRemove.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.Machine.Bridge
{
    public class MaterialRemoveData : IMaterialRemoveData
    {
        public bool Enable { get; set; }
        public int MinNumCells { get; set; } = 16;
        public int SectionsX100mm { get; set; } = 3;
        public PanelFragment PanelFragment { get; set; }
        public SectionDivision SectionDivision { get; set; }
    }
}
