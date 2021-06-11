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
    public abstract class SectionVolumeViewModel : SectionElementViewModel, ISectionVolume, BoundedImplicitFunction3d
    {
        static int _seedId;

        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double SizeZ { get; set; }

        public SectionVolumeViewModel() : base()
        {
            Id = _seedId++;
        }

        internal void ApplyAction(ToolActionData toolActionData)
        {
            if (_toolApplications == null) _toolApplications = new List<BoundedImplicitFunction3d>();

            _toolApplications.Add(toolActionData.ToApplication());

            var procFunction = new ImplicitNaryDifference3d() { A = this, BSet = _toolApplications };
            var box = this.GetBound();
            var cubeSize = box.MaxDim / RemovalParameters.NumCells;
            var filterBox = box;

            InternalGeometry = MeshProcessHelper.GenerateMeshBase(procFunction, filterBox, cubeSize);
            OnActionApplied();
        }

        protected abstract void OnActionApplied();

        #region BoundedImplicitFunction3d
        public AxisAlignedBox3d Bounds() => this.GetBound();

        public double Value(ref Vector3d pt) => this.GetBound().SignedDistance(pt);
        #endregion
    }
}
