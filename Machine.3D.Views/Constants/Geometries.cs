using HelixToolkit.Wpf.SharpDX;
using Machine._3D.Views.Interfaces;
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
                var probeData = Machine.ViewModels.Ioc.SimpleIoc<IProbesViewData>.GetInstance();
                var size = 5.0;

                switch (probeData.Size)
                {
                    case Enums.ProbeSize.Size5:     size = 5.0;     break;
                    case Enums.ProbeSize.Size10:    size = 10.0;    break;
                    case Enums.ProbeSize.Size20:    size = 20.0;    break;
                    default:                        size = 5.0;     break;
                }

                switch (probeData.Shape)
                {
                    case Enums.ProbeShape.Sphere:
                        builder.AddSphere(new SharpDX.Vector3(), size);
                        break;
                    case Enums.ProbeShape.Cube:
                        builder.AddBox(new SharpDX.Vector3(), size, size, size);
                        break;
                    case Enums.ProbeShape.Pyramid:
                        builder.AddPyramid(new SharpDX.Vector3(), size, size, true);
                        break;
                    default:
                        break;
                }                

                return builder.ToMesh();
            }
        }

        public Geometry3D PointIndicator
        {
            get
            {
                var builder = new MeshBuilder();

                builder.AddSphere(new SharpDX.Vector3(), 5.0);

                return builder.ToMesh();
            }
        }

        public Geometry3D PointAndDirIndicator
        {
            get
            {
                var builder = new MeshBuilder();

                builder.AddCylinder(new SharpDX.Vector3(), new SharpDX.Vector3(0.0f, 0.0f, -10.0f), 3.0f);
                builder.AddCone(new SharpDX.Vector3(0.0f, 0.0f, -10.0f), new SharpDX.Vector3(0.0f, 0.0f, -20.0f), 6.0f, true, 32);

                return builder.ToMesh();
            }
        }

        public Geometry3D PointsIndicator
        {
            get
            {
                var builder = new MeshBuilder();

                builder.AddSphere(new SharpDX.Vector3(), 2.0);

                return builder.ToMesh();
            }
        }
    }
}
