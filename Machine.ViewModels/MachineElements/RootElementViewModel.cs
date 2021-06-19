using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class RootElementViewModel : ElementViewModel, IRootElement
    {
        public string AssemblyName { get; set; }
        public RootType RootType { get; set; }

        public RootElementViewModel() : base()
        {
        }
    }
}
