using Machine._3D.Views.Helpers;
using Machine.ViewModels.Handles;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Factories
{
    internal class ElementRotatorFactory : IElementRotatorFactory
    {
        public IElementRotator Create(IMachineElement element) => new ElementRotator(element);
    }
}
