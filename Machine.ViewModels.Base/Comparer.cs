using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Base
{
    interface IValueComparer<T>
    {
        bool IsEqual(T v1, T v2);
    }

    class Comparer<T>
    {
        private static IValueComparer<T> _instance;
        public static IValueComparer<T> Instance 
        {
            get
            {
                if(_instance == null)
                {
                    var type = typeof(T);

                    if (type.IsValueType) _instance = new ValueComparer();
                    else if (type.IsClass) _instance = new ReferenceComparer();
                    else if(type.IsInterface) _instance = new ReferenceComparer();
                    else throw new NotImplementedException();
                }

                return _instance;
            }
        }

        class ValueComparer : IValueComparer<T>
        {
            public bool IsEqual(T v1, T v2) => EqualityComparer<T>.Default.Equals(v1, v2);
        }

        class ReferenceComparer : IValueComparer<T>
        {
            public bool IsEqual(T v1, T v2) => ReferenceEquals(v1, v2);
        }
    }
}
