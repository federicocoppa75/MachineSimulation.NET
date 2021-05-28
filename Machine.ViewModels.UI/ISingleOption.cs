using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface ISingleOption
    {
        string Name { get; }
        bool Selected { get; set; }
    }

    public interface ISingleOption<T>
    {
        T Value { get; }
        bool Selected { get; set; }
    }
}
