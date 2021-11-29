using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.UI.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Factories
{
    public class LinkFactoriesProvider : BaseFactoriesProvider<ILinkFactory, LinkAttribute>, ILinkFactoriesProvider
    {
        protected override ILinkFactory Create(Type type, LinkAttribute a) => new LinkFactory(type, a.Name);
    }
}
