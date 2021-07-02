using Machine.ViewModels.Interfaces.Insertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Insertions
{
    public class InsertedViewModel : InjectedViewModel, IInsertedObject
    {
        public double Diameter { get; set; }
        public double Length { get; set; }
    }
}
