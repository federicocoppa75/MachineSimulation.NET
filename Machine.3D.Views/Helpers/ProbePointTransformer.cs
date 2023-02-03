using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Probing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Helpers
{
    class ProbePointTransformer : IProbePointTransformer    
    {
        protected IMachineElement _probeParent;

        public ProbePointTransformer(IMachineElement probeParent)
        {
            _probeParent = probeParent;
        }

        public Point Transform(Point point, bool gloablToLocal = false)
        {
            var m = _probeParent.GetChainTransformation();

            if(gloablToLocal) m.Invert();
            var p = m.Transform(new System.Windows.Media.Media3D.Point3D() { X = point.X, Y = point.Y, Z = point.Z });

            return new Point() { X = p.X, Y = p.Y, Z = p.Z };
        }
    }
}
