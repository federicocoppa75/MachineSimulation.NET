using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces
{
    public interface IPanelHooker
    {
        ILinkViewModel Link { get; }
    }
}
