using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class InserterElementViewModel : InjectorBaseElementViewModel, IInserterElement
    {
        public double Diameter { get; set; }
        public double Length { get; set; }
        public int LoaderLinkId { get; set; }
        public int DischargerLinkId { get; set; }
    }
}
