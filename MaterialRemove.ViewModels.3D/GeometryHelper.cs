using g3;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels._3D
{
    static class GeometryHelper
    {
        public static Geometry3D Convert(DMesh3 mesh)
        {
            return new MeshGeometry3D()
            {
                Positions = GetPositions(mesh),
                TriangleIndices = GetTriangleIndices(mesh),
                Normals = GetNormals(mesh)
            };
        }

        private static Vector3Collection GetPositions(DMesh3 mesh)
        {
            var positions = new Vector3Collection(mesh.VerticesRefCounts.count);
            var vertices = mesh.VerticesBuffer;

            foreach (int vId in mesh.VerticesRefCounts)
            {
                int i = vId * 3;
                positions.Add(new SharpDX.Vector3((float)vertices[i], (float)vertices[i + 1], (float)vertices[i + 2]));
            }

            return positions;
        }

        private static IntCollection GetTriangleIndices(DMesh3 mesh)
        {
            var tringleindices = new IntCollection(mesh.TrianglesRefCounts.count);
            var triangles = mesh.TrianglesBuffer;

            foreach (int tId in mesh.TrianglesRefCounts)
            {
                int i = tId * 3;
                tringleindices.Add(triangles[i]);
                tringleindices.Add(triangles[i + 1]);
                tringleindices.Add(triangles[i + 2]);
            }

            return tringleindices;
        }

        private static Vector3Collection GetNormals(DMesh3 mesh)
        {
            var normalsList = new Vector3Collection(mesh.VerticesRefCounts.count);
            var normals = mesh.NormalsBuffer;

            foreach (int vId in mesh.VerticesRefCounts)
            {
                int i = vId * 3;
                normalsList.Add(new SharpDX.Vector3(normals[i], normals[i + 1], normals[i + 2]));
            }

            return normalsList;
        }

    }
}
