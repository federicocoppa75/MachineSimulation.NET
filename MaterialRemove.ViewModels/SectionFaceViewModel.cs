using g3;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels
{
    public abstract class SectionFaceViewModel : SectionElementViewModel, ISectionFace, BoundedImplicitFunction3d
    {
        static int _seedId;

        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public Orientation Orientation { get; set; }

        public SectionFaceViewModel() : base()
        {
            Id = _seedId++;
        }

        internal override DMesh3 GenerateMesh()
        {
            var procFunction = new ImplicitNaryDifference3d() { A = this, BSet = ToolApplications };
            var cubeSize = RemovalParameters.CubeSize;
            var filterBox = this.GetFilterBox(RemovalParameters.FilterMargin);

            return MeshProcessHelper.GenerateMeshBase(procFunction, filterBox, cubeSize);
        }

        #region BoundedImplicitFunction3d
        public AxisAlignedBox3d Bounds() => this.GetBound();

        public double Value(ref Vector3d pt) => this.GetDistance(pt);
        #endregion
    }
}
