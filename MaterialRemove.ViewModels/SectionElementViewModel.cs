using g3;
using Machine.ViewModels.Base;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Extensions;
using System.Collections.Generic;

namespace MaterialRemove.ViewModels
{
    public abstract class SectionElementViewModel : BaseViewModel, ISectionElement
    {
        private object _lockObj = new object();
        private List<BoundedImplicitFunction3d> _toolApplications;
        protected List<BoundedImplicitFunction3d> ToolApplications => _toolApplications;
        protected DMesh3 InternalGeometry { get; set; }

        public int Id { get; protected set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double CenterZ { get; set; }
        public bool IsCorrupted => (_toolApplications != null) && (_toolApplications.Count > 0);
        public IRemovalParameters RemovalParameters { get; set; }

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

            _toolApplications.Add(toolActionData.ToApplication());
            if (notify) RisePropertyChanged(nameof(IsCorrupted));
        }
    }
}
