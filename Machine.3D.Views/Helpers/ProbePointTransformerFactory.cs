using Machine.ViewModels.Interfaces.Probing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Helpers
{
    public class ProbePointTransformerFactory : IProbePointTransformerFactory
    {
        public IProbePointTransformer GetTransformer(IProbe probe) => new ProbePointTransformer(probe);

    }
}
