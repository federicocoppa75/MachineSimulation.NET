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

        internal void ApplyAction(ToolActionData toolActionData)
        {
            AddToolActionData(toolActionData);

            var procFunction = new ImplicitNaryDifference3d() { A = this, BSet = ToolApplications };
            var box = this.GetBound();
            var cubeSize = box.MaxDim / RemovalParameters.NumCells;
            var filterBox = box;

            filterBox.Expand(0.0001);

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
                var filterBox = box;

                filterBox.Expand(0.0001);

                InternalGeometry = await TaskHelper.ToAsync(() => MeshProcessHelper.GenerateMeshBase(procFunction, filterBox, cubeSize));
                OnActionApplied();
            });
        }

        protected abstract void OnActionApplied();

        #region BoundedImplicitFunction3d
        public AxisAlignedBox3d Bounds() => this.GetBound();

        public double Value(ref Vector3d pt) => this.GetDistance(pt);
        #endregion
    }
}
