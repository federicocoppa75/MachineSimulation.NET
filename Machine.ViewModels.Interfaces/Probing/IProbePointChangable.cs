using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Probing
{
    public interface IProbePointChangable : IProbePoint
    {
        event EventHandler<double> PointChanged;

        IProbePointChangableTransformer Transformer { get; set; }

        void Detach();
    }
}
