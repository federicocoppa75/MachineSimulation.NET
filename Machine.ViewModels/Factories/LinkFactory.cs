using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Factories
{
    class LinkFactory : ILinkFactory
    {
        private Type _type;

        public string Label { get; private set; }

        public LinkFactory(Type type, string label)
        {
            _type = type;
            Label = label;
        }

        public ILinkViewModel Create()
        {
            return (ILinkViewModel)Activator.CreateInstance(_type);
        }
    }
}
