using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    interface IValueProvider<T>
    {
        T Value { get; set; }

        bool IsEqual(T value);
    }
}
