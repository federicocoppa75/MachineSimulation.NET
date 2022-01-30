using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public class StringOptionProxy : OptionProxy<string>
    {
        public StringOptionProxy(Func<IEnumerable<string>> getOptions, Func<string> getValue, Action<string> setValue) : base(getOptions, getValue, setValue)
        {
        }

        public override bool IsEqual(string value) => string.Compare(value, Value) == 0;

        public override bool TryToParse(string value)
        {
            Value = value;
            return true;
        }
    }
}
