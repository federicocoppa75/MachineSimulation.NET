using MaterialRemove.Interfaces;
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
    }
}
