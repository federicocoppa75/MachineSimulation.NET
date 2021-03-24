using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface IOptionProvider
    {
        string Name { get; set; }

        IEnumerable<ISingleOption> Options { get; }

        bool TryToParse(string value);
    }

    public interface IOptionProvider<T>
    {
        IEnumerable<T> Options { get; }
        T Value { get; set; }

        bool TryToParse(string value);
    }
}
