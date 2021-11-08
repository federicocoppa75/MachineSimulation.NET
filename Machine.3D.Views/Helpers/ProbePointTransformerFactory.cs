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
    public class ProbePointTransformerFactory : IProbePointTransformerFactory
    {
        public IProbePointTransformer GetTransformer(IMachineElement probeParent)
        {
            var links = GetLinks(probeParent);
            var movables = GetMovables(probeParent);

            if((links.Count() > 0) || (movables.Count() > 0))
            {
                return new ProbePointChangableTransformer(probeParent, links, movables);
            }
            else
            {
                return new ProbePointTransformer(probeParent);
            }
        }

        #region implementation
        private static IList<ILinkViewModel> GetLinks(IMachineElement machineElement)
        {
            var list = new List<ILinkViewModel>();
            var me = machineElement;

            while (me.Parent != null)
            {
                if (me.LinkToParent != null) list.Add(me.LinkToParent);
                me = me.Parent;
            }

            return list;
        }

        private static IList<IMovablePanel> GetMovables(IMachineElement machineElement)
        {
            var list = new List<IMovablePanel>();
            var me = machineElement;

            while (me.Parent != null)
            {
                if (me is IMovablePanel mp) list.Add(mp);
                me = me.Parent;
            }

            return list;
        }
        #endregion

    }
}
