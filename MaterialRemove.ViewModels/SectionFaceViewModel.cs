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
            var cubeSize = RemovalParameters.CubeSize;
            var filterBox = this.GetFilterBox();

            InternalGeometry = MeshProcessHelper.GenerateMeshBase(procFunction, filterBox, cubeSize);
            OnActionApplied();
        }

        internal Task ApplyActionAsync(ToolActionData toolActionData)
        {
            return Task.Run(async () =>
            {
                AddToolActionData(toolActionData);

                var procFunction = new ImplicitNaryDifference3d() { A = this, BSet = ToolApplications };
                var cubeSize = RemovalParameters.CubeSize;
                var filterBox = await TaskHelper.ToAsync(() => this.GetFilterBox());

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
