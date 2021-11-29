using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Factories
{
    public interface ILinkFactory
    {
        string Label { get; }

        ILinkViewModel Create();
    }
}
