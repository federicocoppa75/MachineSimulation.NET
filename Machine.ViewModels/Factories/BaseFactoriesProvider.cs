using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.ViewModels.Factories
{
    public abstract class BaseFactoriesProvider<I, A> where A : class
    {
        private IEnumerable<I> _factories;
        public IEnumerable<I> Factories => _factories ?? (_factories = CreateFactories());

        private IEnumerable<I> CreateFactories()
        {
            var list = new List<I>();
            var types = AppDomain.CurrentDomain
                     .GetAssemblies()
                     .SelectMany((a) => a.GetTypes())
                     .Where((t) => t.IsClass)
                     .ToList();

            foreach (var type in types)
            {
                var a = GetAttribute(type);

                if (a != null)
                {
                   list.Add(Create(type, a));
                }
            }

            return Process(list);
        }

        protected abstract I Create(Type type, A a);

        protected virtual IEnumerable<I> Process(IEnumerable<I> factories) => factories;

        private static A GetAttribute(Type t)
        {
            return t.GetCustomAttributes(typeof(A), false)
                     .Select(o => o as A)
                     .FirstOrDefault();
        }
    }
}
