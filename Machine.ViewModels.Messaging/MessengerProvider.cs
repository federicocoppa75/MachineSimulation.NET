using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messaging
{
    static class MessengerProvider
    {
        static List<MessengerImplementation> _istances;
        public static ICollection<MessengerImplementation> IStances => _istances ?? (_istances = new List<MessengerImplementation>());

        public static IMessengerImplementation<T> GetInstance<T>() => MessengerImplementation<T>.GetInstance();
    }
}
