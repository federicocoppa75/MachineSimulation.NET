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
    class ProbePointChangableTransformer : ProbePointTransformer, IProbePointTransformer, IProbePointChangableTransformer
    {
        IList<ILinkViewModel> _links;
        IList<IMovablePanel> _movables;

        public event EventHandler<double> TransformerChanged;

        public ProbePointChangableTransformer(IMachineElement probeParent, IList<ILinkViewModel> links, IList<IMovablePanel> movables) : base(probeParent)
        {
            _links = links;
            _movables = movables;

            AttachToLinks();
            AttachToMovable();
        }

        public void Detach()
        {
            foreach (var item in _links)
            {
                item.ValueChanged -= OnLinkValueChanged;
            }

            foreach (var item in _movables)
            {
                item.ValueChanged -= OnMovableChanged;
            }
        }

        private void AttachToLinks()
        {
            foreach (var item in _links)
            {
                item.ValueChanged += OnLinkValueChanged;
            }
        }

        private void AttachToMovable()
        {
            foreach (var item in _movables)
            {
                item.ValueChanged += OnMovableChanged;
            }
        }

        private void OnMovableChanged(object sender, double e) => Task.Run(() => TransformerChanged?.Invoke(sender, e));


        private void OnLinkValueChanged(object sender, double e) => Task.Run(() => TransformerChanged?.Invoke(sender, e));

    }
}
