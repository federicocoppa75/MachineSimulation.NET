using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messaging
{
    class ActionHandle<T>
    {
        WeakReference _recipient;
        Action<T> _action;

        public ActionHandle(object recipient, Action<T> action)
        {
            _recipient = new WeakReference(recipient);
            _action = action;
        }

        public bool TryGetAction(out Action<T> action)
        {
            action = _recipient.IsAlive ? _action : null;

            return _recipient.IsAlive;
        }

        public bool HasSameRecipient(object recipient)
        {
            var r = _recipient.Target;

            return (r != null) && ReferenceEquals(r, recipient);
        }
    }
}
