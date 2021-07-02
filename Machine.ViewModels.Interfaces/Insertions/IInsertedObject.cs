using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Insertions
{
    public interface IInsertedObject : IInjectedObject
    {
        double Diameter { get; set; }
        double Length { get; set; }
    }
}
