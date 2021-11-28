using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Factories
{
    public interface IMachineElementFactoriesProvider
    {
        IEnumerable<IMachineElementFactory> Factories { get; }
    }
}
