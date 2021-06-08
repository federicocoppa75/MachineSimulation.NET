using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Base
{
    public interface IMessenger
    {
        void Register<TMessage>(object recipient, Action<TMessage> action);
        void Send<TMessage>(TMessage message);
        void Unregister(object recipient);
    }
}
