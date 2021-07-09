using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messaging
{
    abstract class MessengerImplementation : IMessengerImplementation
    {
        static int _seedId = 0;

        public abstract void Unregister(object recipient);

        protected int GetKey() => _seedId++;
    }

    class MessengerImplementation<T> : MessengerImplementation, IMessengerImplementation<T>
    {
        static MessengerImplementation<T> _instance;
        Dictionary<int, ActionHandle<T>> _actions = new Dictionary<int, ActionHandle<T>>();

        public static MessengerImplementation<T> GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MessengerImplementation<T>();
                MessengerProvider.IStances.Add(_instance);
            }

            return _instance;
        }

        public void Register(object recipient, Action<T> action)
        {
            _actions.Add(GetKey(), new ActionHandle<T>(recipient, action));
        }

        public void Send(T message)
        {
            var toRemove = new List<int>();

            foreach (var item in _actions)
            {
                if(item.Value.TryGetAction(out Action<T> action))
                {
                    action(message);
                }
                else
                {
                    toRemove.Add(item.Key);
                }
            }

            if (toRemove.Count > 0) Remove(toRemove);
        }

        public override void Unregister(object recipient)
        {
            var toRemove = new List<int>();

            foreach (var item in _actions)
            {
                if (item.Value.HasSameRecipient(recipient)) toRemove.Add(item.Key);
            }

            if (toRemove.Count > 0) Remove(toRemove);
        }

        private void Remove(IEnumerable<int> keys)
        {
            foreach (var k in keys)
            {
                _actions.Remove(k);
            }
        }
    }
}
