using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messaging
{
    interface IMessengerImplementation
    {
        void Unregister(object recipient);
    }

    interface IMessengerImplementation<T> : IMessengerImplementation
    {
        void Register(object recipient, Action<T> action);
        void Send(T message);
    }
}
