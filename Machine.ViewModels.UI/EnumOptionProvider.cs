using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine.ViewModels.UI
{
    public class EnumOptionProvider<T> : OptionProvider<T> where T : struct, Enum
    {
        public override IEnumerable<ISingleOption> Options => Enum.GetValues(typeof(T)).Cast<T>().Select(t => new SingleOption<T>() { Value = t, ValueProvider = this });

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
