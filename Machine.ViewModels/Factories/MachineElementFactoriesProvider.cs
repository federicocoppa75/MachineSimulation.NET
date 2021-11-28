using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.UI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.ViewModels.Factories
{
    public class MachineElementFactoriesProvider : IMachineElementFactoriesProvider
    {
        private IEnumerable<IMachineElementFactory> _factories;
        public IEnumerable<IMachineElementFactory> Factories => _factories ?? (_factories = CreateFactories());

        private IEnumerable<IMachineElementFactory> CreateFactories()
        {
            var list = new List<IMachineElementFactory>();
            var types = AppDomain.CurrentDomain
                     .GetAssemblies()
                     .SelectMany((a) => a.GetTypes())
                     .Where((t) => t.IsClass)
                     .ToList();

            foreach (var type in types)
            {
                var a = GetAttribute<MachineStructAttribute>(type);

                if (a != null)
                {
                    list.Add(new MachineElementFactory(type, a.Name, a.Index, a.Root));
                }
            }

            return list.OrderBy(o => o.Index);
        }

        private static T GetAttribute<T>(Type t) where T : MachineStructAttribute
        {
            return t.GetCustomAttributes(typeof(T), false)
                     .Select(o => o as T)
                     .FirstOrDefault();
        }
    }
}
