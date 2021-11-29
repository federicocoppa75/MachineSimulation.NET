using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.UI.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    [MachineStruct("Root element", 0, true)]
    public class RootElementViewModel : ElementViewModel, IRootElement
    {
        private string _assemblyName;
        public string AssemblyName 
        { 
            get => _assemblyName; 
            set => Set(ref _assemblyName, value, nameof(AssemblyName)); 
        }

        private RootType _rootType;
        public RootType RootType 
        { 
            get => _rootType; 
            set => Set(ref _rootType, value, nameof(RootType)); 
        }

        public RootElementViewModel() : base()
        {
        }
    }
}
