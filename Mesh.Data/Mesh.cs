using System;
using System.Collections.Generic;
using System.Text;

namespace Mesh.Data
{
    public class Mesh
    {
        public int MeshID { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
