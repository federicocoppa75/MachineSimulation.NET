using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Constants
{
    public class Geometries
    {
        public Geometry3D Debug
        {
            get
            {
                var builder = new MeshBuilder();

                builder.AddSphere(new SharpDX.Vector3(), 10);

                return builder.ToMesh();
            }
        }

        public Geometry3D Probe 
        {
            get 
            {
                var builder = new MeshBuilder();

                builder.AddSphere(new SharpDX.Vector3(), 10);

                return builder.ToMesh();
            }
        }
    }
}
