using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messaging
{
    public class Messenger : IMessenger
    {
        //public void Register<TMessage>(object recipient, Action<TMessage> action) => MessengerImplementation.GetInstance<TMessage>().Register(recipient, action);

        //public void Send<TMessage>(TMessage message) => MessengerImplementation.GetInstance<TMessage>().Send(message);

        //public void Unregister(object recipient) => MessengerImplementation.Unregister(recipient);

        public void Register<TMessage>(object recipient, Action<TMessage> action) => MessengerProvider.GetInstance<TMessage>().Register(recipient, action);

        public void Send<TMessage>(TMessage message) => MessengerProvider.GetInstance<TMessage>().Send(message);

        public void Unregister(object recipient)
        {
            foreach (var item in MessengerProvider.IStances)
            {
                item.Unregister(recipient);
            }
        }
    }
}
