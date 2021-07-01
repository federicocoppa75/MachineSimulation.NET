using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IInserterElement : IInjectorElement
    {
        double Diameter { get; set; }
        double Length { get; set; }
        int LoaderLinkId { get; set; }
        int DischargerLinkId { get; set; }
    }
}
