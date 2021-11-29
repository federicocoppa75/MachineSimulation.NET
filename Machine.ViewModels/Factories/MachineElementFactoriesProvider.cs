using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.UI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.ViewModels.Factories
{
    public class MachineElementFactoriesProvider : BaseFactoriesProvider<IMachineElementFactory, MachineStructAttribute>, IMachineElementFactoriesProvider
    {
        protected override IMachineElementFactory Create(Type type, MachineStructAttribute a) => new MachineElementFactory(type, a.Name, a.Index, a.Root);

        protected override IEnumerable<IMachineElementFactory> Process(IEnumerable<IMachineElementFactory> factories) => factories.OrderBy(o => o.Index);
    }
}
