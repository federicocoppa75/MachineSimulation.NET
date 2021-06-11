using g3;
using Machine.ViewModels.Base;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels
{
    public abstract class SectionElementViewModel : BaseViewModel ,ISectionElement
    {
        protected List<BoundedImplicitFunction3d> _toolApplications;
        protected DMesh3 InternalGeometry { get; set; }

        public int Id { get; protected set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double CenterZ { get; set; }
        public bool IsCorrupted => (_toolApplications != null) && (_toolApplications.Count > 0);
        public IRemovalParameters RemovalParameters { get; set; }
    }
}
