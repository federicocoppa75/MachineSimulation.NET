using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.Interfaces
{
    public struct ToolConeActionData
    {
        public float MinRadius { get; set; }
        public float MaxRadius { get; set; }
        public float Length { get; set; }
        public Orientation Orientation { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float DX { get; set; }
        public float DY { get; set; }
        public float DZ { get; set; }
    }
}
