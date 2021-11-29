using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class LinkAttribute : Attribute
    {
        public string Name { get; private set; }

        public LinkAttribute(string name)
        {
            Name = name;
        }
    }
}
