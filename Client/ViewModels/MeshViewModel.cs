using g3;
using GalaSoft.MvvmLight;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Client.ViewModels
{
    public class MeshViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }

        private MeshGeometry3D _meshGeometry;
        public MeshGeometry3D MeshGeometry => _meshGeometry ?? (_meshGeometry = GetMeshGeometry());

        public MeshViewModel() : base()
        {
        }

        private MeshGeometry3D GetMeshGeometry()
        {
            byte[] data = null;
            var mres = new ManualResetEventSlim();

            Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"https://localhost:44306/api/Models/{Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsAsync<Mesh.Data.Mesh>();

                        data = content.Data;
                    }
                }

                mres.Set();
            });

            mres.Wait();
            mres.Reset();

            StandardMeshReader reader = new StandardMeshReader() { MeshBuilder = new DMesh3Builder() };

            using (var stream = new MemoryStream(data))
            {
                reader.Read(stream, "stl", new ReadOptions());
            }

            var mesh = (reader.MeshBuilder as DMesh3Builder).Meshes[0];
            var mg = ToMeshGeometry3D(mesh);

            return mg;
        }

        private MeshGeometry3D ToMeshGeometry3D(DMesh3 src)
        {
            MeshGeometry3D dest = new MeshGeometry3D();

            if (!src.IsCompact) src.CompactInPlace();

            var vertices = src.VerticesBuffer;

            foreach (int vId in src.VerticesRefCounts)
            {
                int i = vId * 3;
                dest.Positions.Add(new Point3D(vertices[i], vertices[i + 1], vertices[i + 2]));
            }

            var triangles = src.TrianglesBuffer;

            foreach (int tId in src.TrianglesRefCounts)
            {
                int i = tId * 3;
                dest.TriangleIndices.Add(triangles[i]);
                dest.TriangleIndices.Add(triangles[i + 1]);
                dest.TriangleIndices.Add(triangles[i + 2]);
            }

            return dest;
        }
    }
}
