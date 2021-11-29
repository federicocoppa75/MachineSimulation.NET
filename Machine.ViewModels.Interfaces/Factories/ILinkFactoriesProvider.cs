using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Factories
{
    public interface ILinkFactoriesProvider
    {
        IEnumerable<ILinkFactory> Factories { get; }
    }
}
