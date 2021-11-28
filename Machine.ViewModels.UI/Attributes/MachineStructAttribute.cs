using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class MachineStructAttribute : Attribute
    {
        public string Name { get; private set; }
        public int Index { get; set; }
        public bool Root { get; private set; }

        public MachineStructAttribute(string name, int index = -1, bool root = false)
        {
            Name = name;
            Index = index;
            Root = root;
        }
    }
}
