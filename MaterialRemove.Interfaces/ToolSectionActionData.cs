using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.Interfaces
{
    public struct ToolSectionActionData
    {
        public float PX { get; set; }
        public float PY { get; set; }
        public float PZ { get; set; }
        public float DX { get; set; }
        public float DY { get; set; }
        public float DZ { get; set; }
        public float L { get; set; }
        public float W { get; set; }
        public float H { get; set; }
        public Orientation FixBaseAx { get; set; }
    }
}
