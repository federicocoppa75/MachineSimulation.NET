using Machine.ViewModels.Interfaces.Probing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.Probing
{
    public class ProbePointChangableViewModel : PointProbeViewModel, IProbePointChangable
    {

        #region IProbePointChangable
        private IProbePointChangableTransformer _transformer;
        public IProbePointChangableTransformer Transformer
        {
            get => _transformer;
            set
            {
                if (_transformer != null)
                {
                    _transformer.TransformerChanged -= OnTransformerChanged;
                }

                if (Set(ref _transformer, value, nameof(Transformer)) && (_transformer != null))
                {
                    _transformer.TransformerChanged += OnTransformerChanged;
                }
            }
        }

        public event EventHandler<double> PointChanged;

        public void Detach()
        {
            if (Transformer != null)
            {
                Transformer.Detach();
                Transformer = null;
            }
        }
        #endregion

        private void OnTransformerChanged(object sender, double e)
        {
            Task.Run(() =>
            {
                var p = _transformer.Transform(new Interfaces.Probing.Point() { X = RelativeX, Y = RelativeY, Z = RelativeZ });

                X = p.X;
                Y = p.Y;
                Z = p.Z;

                PointChanged?.Invoke(sender, e);
            });
        }
    }
}
