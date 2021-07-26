using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Ioc
{
    public static class SimpleIoc<T>
    {
        private const string _singleKey = "1";
        private static Dictionary<string, T> _instances;

        static SimpleIoc()
        {
            _instances = new Dictionary<string, T>();
        }

        public static T GetInstance() => GetInstance(_singleKey);

        public static void Register<Y>() where Y : class, T, new()
        {
             Register<Y>(_singleKey);
        }

        public static void Register<Y>(Y instance) where Y : class, T
        {
            Register<Y>(_singleKey, instance);
        }

        public static T GetInstance(string key) => _instances[key];

        public static void Register<Y>(string key) where Y : class, T, new()
        {
            _instances[key] = new Y();
        }

        public static void Register<Y>(string key, Y instance) where Y : class, T
        {
            _instances[key] = instance;
        }

        public static IEnumerable<string> GetKeys() => _instances.Keys;

        public static IEnumerable<T> GetInstances() => _instances.Values;

        public static bool HasInstance() => _instances.Count > 0;

        public static bool HasInstance(string key) => _instances.ContainsKey(key);
    }
}
