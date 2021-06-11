using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels.Extensions
{
    static class MeshProcessHelper
    {
        internal static DMesh3 GenerateMeshBase(BoundedImplicitFunction3d root, AxisAlignedBox3d filterBox, double cubeSize)
        {
            MarchingCubes c = new MarchingCubes();
            c.Implicit = root;
            c.RootMode = MarchingCubes.RootfindingModes.LerpSteps;      // cube-edge convergence method
            c.RootModeSteps = 5;                                        // number of iterations
            c.Bounds = filterBox;//_sideFilterBox;
            c.CubeSize = cubeSize; // _cubeSize / 4.0;
            c.Generate();
            MeshNormals.QuickCompute(c.Mesh);                           // generate normals

            return c.Mesh;
        }
    }
}
