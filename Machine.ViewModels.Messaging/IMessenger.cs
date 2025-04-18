﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messaging
{
    public interface IMessenger
    {
        void Register<TMessage>(object recipient, Action<TMessage> action);
        void Send<TMessage>(TMessage message);
        void Unregister<TMessage>(object recipient);
        void Unregister(object recipient);
    }
}
