using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Factories
{
    class MachineElementFactory : IMachineElementFactory
    {
        private Type _type;

        public int Index { get; private set; }
        public string Label { get; private set; }
        public bool IsRoot { get; private set; }

        public MachineElementFactory(Type type, string label, int index, bool root)
        {
            _type = type;
            Label = label;
            Index = index;
            IsRoot = root;
        }

        public IMachineElement Create()
        {
            return (IMachineElement)Activator.CreateInstance(_type);
        }
    }
}
