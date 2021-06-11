using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRVM = MaterialRemove.ViewModels;
using SharpDX;
using MaterialRemove.Interfaces;

namespace MaterialRemove.ViewModels._3D
{
    class SectionFaceViewModel : MRVM.SectionFaceViewModel
    {
        private Geometry3D _geometry;

        public Geometry3D Geometry
        {
            get => _geometry ?? (_geometry = GetIntactGeometry()); 
            set => Set(ref _geometry, value, nameof(Geometry));
        }

        protected override void OnActionApplied() => Geometry = GeometryHelper.Convert(InternalGeometry);

        private Geometry3D GetIntactGeometry()
        {
            var builder = new MeshBuilder();
            var v = GetVertexes();

            builder.AddQuad(v[0], v[1], v[2], v[3]);
            return builder.ToMesh();
        }

        private Vector3[] GetVertexes()
        {
            var dx = (float)SizeX / 2.0f;
            var dy = (float)SizeY / 2.0f;
            var c = new Vector3((float)CenterX, (float)CenterY, (float)CenterZ);

            switch (Orientation)
            {
                case Orientation.XPos:
                    return new Vector3[]
                    {
                        new Vector3(c.X, c.Y - dx, c.Z + dy),
                        new Vector3(c.X, c.Y - dx, c.Z - dy),
                        new Vector3(c.X, c.Y + dx, c.Z - dy),
                        new Vector3(c.X, c.Y + dx, c.Z + dy)
                    };

                case Orientation.XNeg:
                    return new Vector3[]
                    {
                        new Vector3(c.X, c.Y + dx, c.Z + dy),
                        new Vector3(c.X, c.Y + dx, c.Z - dy),
                        new Vector3(c.X, c.Y - dx, c.Z - dy),
                        new Vector3(c.X, c.Y - dx, c.Z + dy)
                    };

                case Orientation.YPos:
                    return new Vector3[]
                    {
                        new Vector3(c.X + dx, c.Y, c.Z + dy),
                        new Vector3(c.X + dx, c.Y, c.Z - dy),
                        new Vector3(c.X - dx, c.Y, c.Z - dy),
                        new Vector3(c.X - dx, c.Y, c.Z + dy)
                    };

                case Orientation.YNeg:
                    return new Vector3[]
                    {
                        new Vector3(c.X - dx, c.Y, c.Z + dy),
                        new Vector3(c.X - dx, c.Y, c.Z - dy),
                        new Vector3(c.X + dx, c.Y, c.Z - dy),
                        new Vector3(c.X + dx, c.Y, c.Z + dy)
                    };

                case Orientation.ZPos:
                    return new Vector3[]
                    {
                        new Vector3(c.X - dx, c.Y + dy, c.Z),
                        new Vector3(c.X - dx, c.Y - dy, c.Z),
                        new Vector3(c.X + dx, c.Y - dy, c.Z),
                        new Vector3(c.X + dx, c.Y + dy, c.Z)
                    };

                case Orientation.ZNeg:
                    return new Vector3[]
                    {
                        new Vector3(c.X + dx, c.Y + dy, c.Z),
                        new Vector3(c.X + dx, c.Y - dy, c.Z),
                        new Vector3(c.X - dx, c.Y - dy, c.Z),
                        new Vector3(c.X - dx, c.Y + dy, c.Z)
                    };

                default:
                    throw new ArgumentException();
            }
        }
    }
}
