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
            AddToolActionData(toolActionData);

            var procFunction = new ImplicitNaryDifference3d() { A = this, BSet = ToolApplications };
            var box = this.GetBound();
            var cubeSize = box.MaxDim / RemovalParameters.NumCells;
            var filterBox = GetDecreaseBound(0.1);

            InternalGeometry = MeshProcessHelper.GenerateMeshBase(procFunction, filterBox, cubeSize);
            OnActionApplied();
        }

        internal Task ApplyActionAsync(ToolActionData toolActionData)
        {
            return Task.Run(async () =>
            {
                AddToolActionData(toolActionData);

                var procFunction = new ImplicitNaryDifference3d() { A = this, BSet = ToolApplications };
                var box = await TaskHelper.ToAsync(() => this.GetBound());
                var cubeSize = box.MaxDim / RemovalParameters.NumCells;
                var filterBox = await TaskHelper.ToAsync(() => GetDecreaseBound(0.1));

                InternalGeometry = await TaskHelper.ToAsync(() => MeshProcessHelper.GenerateMeshBase(procFunction, filterBox, cubeSize));
                OnActionApplied();
            });
        }

        private AxisAlignedBox3d GetExpandedBound(double radius)
        {
            var box = this.GetBound();
            box.Expand(radius);
            return box;
        }

        private AxisAlignedBox3d GetDecreaseBound(double radius)
        {
            var box = this.GetBound();
            var v = new Vector3d(radius, radius, radius);

            box.Min += v;
            box.Max -= v;

            return box;
        }

        protected abstract void OnActionApplied();

        #region BoundedImplicitFunction3d
        public AxisAlignedBox3d Bounds() => this.GetBound();

        public double Value(ref Vector3d pt) => GetExpandedBound(5.0).SignedDistance(pt);
        #endregion
    }
}
