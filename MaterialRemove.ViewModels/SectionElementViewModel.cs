using g3;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using MaterialRemove.ViewModels.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MaterialRemove.ViewModels
{
    public abstract class SectionElementViewModel : BaseViewModel, ISectionElement
    {
        private object _lockObj = new object();
        private List<BoundedImplicitFunction3d> _toolApplications;
        protected List<BoundedImplicitFunction3d> ToolApplications => _toolApplications;
        protected DMesh3 InternalGeometry { get; set; }

        protected IProgressState _stateProgressState;

        public int Id { get; protected set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double CenterZ { get; set; }
        public bool IsCorrupted => (_toolApplications != null) && (_toolApplications.Count > 0);
        public IRemovalParameters RemovalParameters { get; set; }

        public SectionElementViewModel() : base()
        {
            _stateProgressState = GetInstance<IProgressState>();
        }

        protected void AddToolActionData(ToolActionData toolActionData)
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

            _toolApplications.Add(toolActionData.ToApplication(GetIndex()));
            if (notify) RisePropertyChanged(nameof(IsCorrupted));
        }

        private int GetIndex() => (_stateProgressState != null) ? _stateProgressState.ProgressIndex : -1;

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
