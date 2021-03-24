using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVB = Machine.ViewModels.Base;

namespace Machine.Viewer.Helpers
{
    class MessengerImplementation : MVB.IMessenger
    {
        public void Register<TMessage>(object recipient, Action<TMessage> action) => Messenger.Default.Register<TMessage>(recipient, action);

        public void Send<TMessage>(TMessage message) => Messenger.Default.Send<TMessage>(message);

        public void Unregister(object recipient) => Messenger.Default.Unregister(recipient);
    }
}
