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
    class ProbePointTransformer : IProbePointTransformer, IProbePointChangableTransformer
    {
        IProbe _probe;
        IList<ILinkViewModel> _links;

        public event EventHandler<double> TransformerChanged;

        public ProbePointTransformer(IProbe probe)
        {
            _probe = probe;
            _links = GetLinks(probe as IMachineElement);

            AttachToLinks();
        }

        public Point Transform(Point point, bool gloablToLocal = false)
        {
            var m = (_probe as IMachineElement).GetChainTransformation();

            if(gloablToLocal) m.Invert();
            var p = m.Transform(new System.Windows.Media.Media3D.Point3D() { X = point.X, Y = point.Y, Z = point.Z });

            return new Point() { X = p.X, Y = p.Y, Z = p.Z };
        }

        public void Detach()
        {
            foreach (var item in _links)
            {
                item.ValueChanged -= OnLinkValueChanged;
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

        private void AttachToLinks()
        {
            foreach (var item in _links)
            {
                item.ValueChanged += OnLinkValueChanged;
            }
        }

        private void OnLinkValueChanged(object sender, double e) => Task.Run(() => TransformerChanged?.Invoke(sender, e));
        #endregion
    }
}
