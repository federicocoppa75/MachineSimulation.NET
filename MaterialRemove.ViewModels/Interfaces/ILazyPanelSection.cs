using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels.Interfaces
{
    internal interface ILazyPanelSection : IPanelSection
    {
        IPanelSection ThresholdToExplode { get; }

        IList<IPanelSection> GetSubSections();
    }
}
