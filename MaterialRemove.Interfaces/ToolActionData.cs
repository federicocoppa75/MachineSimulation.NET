using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.Interfaces
{
    public struct ToolActionData
    {
        public float Radius { get; set; }
        public float Length { get; set; }
        public Orientation Orientation { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
