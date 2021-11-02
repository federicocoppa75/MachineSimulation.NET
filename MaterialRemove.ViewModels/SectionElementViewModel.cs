using g3;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using MaterialRemove.ViewModels.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVMIP = Machine.ViewModels.Interfaces.Probing;

namespace MaterialRemove.ViewModels
{
    public abstract class SectionElementViewModel : BaseViewModel, ISectionElement, IProbableElementProxy
    {
        private object _lockObj = new object();
        private List<BoundedImplicitFunction3d> _toolApplications;
        private MVMIP.IProbableElement _probableElement;

        protected List<BoundedImplicitFunction3d> ToolApplications => _toolApplications;
        protected DMesh3 InternalGeometry { get; set; }

        public int Id { get; protected set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double CenterZ { get; set; }
        public bool IsCorrupted => (_toolApplications != null) && (_toolApplications.Count > 0);
        public IRemovalParameters RemovalParameters { get; set; }

        public SectionElementViewModel() : base()
        {
        }

        public void ApplyAction(BoundedImplicitFunction3d toolApplication)
        {
            AddToolApplication(toolApplication);
            InternalGeometry = GenerateMesh();
            OnActionApplied();
        }

        public void AddProbePoint(MVMIP.Point point) => _probableElement?.AddProbePoint(point);
        public void SetProbableElement(MVMIP.IProbableElement probableElement) => _probableElement = probableElement;

        internal Task ApplyActionAsync(BoundedImplicitFunction3d toolApplication)
        {
            return Task.Run(async () =>
            {
                AddToolApplication(toolApplication);
                InternalGeometry = await Task.Run(() => GenerateMesh());
                OnActionApplied();
            });
        }

        internal void RemoveAction(int actionIndex)
        {
            var n = RemoveActionData(actionIndex);

            if (n > 0)
            {
                InternalGeometry = IsCorrupted ? GenerateMesh() : null;
                OnActionApplied();
            }
        }

        protected abstract void OnActionApplied();

        protected abstract DMesh3 GenerateMesh();

        public void AddToolApplication(BoundedImplicitFunction3d toolApplication)
        {
            bool notify = false;

            lock (_lockObj)
            {
                if (_toolApplications == null)
                {
                    _toolApplications = new List<BoundedImplicitFunction3d>();
                    notify = true;
                }
            }

            _toolApplications.Add(toolApplication);
            if (notify) RisePropertyChanged(nameof(IsCorrupted));
        }

        protected int RemoveActionData(int actionIndex)
        {
            var result = 0;

            if (_toolApplications != null)
            {
                result = _toolApplications.RemoveAll(b => (b as IIndexed).Index == actionIndex);

                if (result == 0) RisePropertyChanged(nameof(IsCorrupted));
            }

            return result;
        }
    }
}
