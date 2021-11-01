using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Probing
{
    public interface IProbePointChangable : IProbePoint
    {
        public IProbePointChangableTransformer Transformer { get; set; }
    }
}
