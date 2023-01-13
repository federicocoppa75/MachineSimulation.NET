using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.ViewModels.UI
{
    public class EnumOptionProxy<T> : OptionProxy<T> where T : struct, Enum
    {
        public EnumOptionProxy(Func<IEnumerable<T>> getOptions, Func<T> getValue, Action<T> setValue) : base(getOptions, getValue, setValue)
        {
        }

        public EnumOptionProxy(Func<T> getValue, Action<T> setValue) : this(() => Enum.GetValues(typeof(T)).Cast<T>(), getValue, setValue)
        {
        }

        public override bool IsEqual(T value) => EqualityComparer<T>.Default.Equals(Value, value);

        public override bool TryToParse(string value)
        {
            var result = false;

            if (Enum.TryParse<T>(value, out T v))
            {
                Value = v;
                result = true;
            }

            return result;
        }
    }
}
