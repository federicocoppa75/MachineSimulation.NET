using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messaging
{
    static class MessengerProvider
    {
        static object _lockObj = new object();
        static List<IMessengerImplementation> _istances;

        static MessengerProvider()
        {
            _istances = new List<IMessengerImplementation>();
        }

        public static ICollection<IMessengerImplementation> IStances => _istances;

        public static IMessengerImplementation<T> GetInstance<T>() => MessengerImplementation<T>.GetInstance();
    }
}
