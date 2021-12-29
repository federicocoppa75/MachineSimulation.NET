using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Data.Factories
{
    internal static class SimpleFactory<T>
    {
        static Func<T> _factory = null;

        public static T Create()
        {
            return (_factory != null) ? _factory() : throw new Exception("Error: object not initialized! Use register method to initialize");   
        }

        public static void Register<Y>() where Y : T, new()
        {
            _factory = () => new Y();
        }
    }
}
