using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class RootElementViewModel : ElementViewModel
    {
        public string AssemblyName { get; set; }
        public RootType RootType { get; set; }

        public RootElementViewModel() : base()
        {
        }
    }
}
