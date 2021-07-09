using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Machine.ViewModels.Messaging
{
    class MessengerImplementation<T> : IMessengerImplementation<T>
    {
        static int _seedId = 0;
        static object _lockObj = new object();
        static MessengerImplementation<T> _instance;
        ConcurrentDictionary<int, ActionHandle<T>> _actions = new ConcurrentDictionary<int, ActionHandle<T>>();

        public static MessengerImplementation<T> GetInstance()
        {
            lock (_lockObj)
            {
                if (_instance == null)
                {
                    _instance = new MessengerImplementation<T>();
                    MessengerProvider.IStances.Add(_instance);
                }
            }

            return _instance;
        }

        public void Register(object recipient, Action<T> action) => _actions.TryAdd(GetKey(), new ActionHandle<T>(recipient, action));

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

        public void Unregister(object recipient)
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
            Task.Run(() =>
            {
                foreach (var k in keys)
                {
                    _actions.TryRemove(k, out ActionHandle<T> v);
                }
            });
        }

        protected int GetKey() => Interlocked.Increment(ref _seedId);
    }
}
