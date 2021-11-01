using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Probing
{
    public interface IProbePointChangableTransformer : IProbePointTransformer
    {

        event EventHandler<double> TransformerChanged;

        void Detach();
    }
}
