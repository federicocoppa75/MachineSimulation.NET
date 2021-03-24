using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.ViewModels.UI
{
    public class RegisteredOptionProvider<T> : OptionProvider<T>, IValueProvider<T> where T : INameProvider
    {
        private IEnumerable<ISingleOption> _options;
        public override IEnumerable<ISingleOption> Options
        {
            get
            {
                if(_options == null)
                {
                    _options = Ioc.SimpleIoc<T>.GetInstances().Select(o => new SingleOption<T>(() => o.Name) { Value = o, ValueProvider = this });
                    _options.FirstOrDefault().Selected = true;
                }

                return _options;
            }
        }
        public override bool IsEqual(T value) => ReferenceEquals(value, Value);

        public override bool TryToParse(string value)
        {
            var result = false;
            var obj = Options.FirstOrDefault(o => string.Compare(o.Name, value) == 0);

            if(obj != null)
            {
                obj.Selected = true;
                result = true;
            }

            return result;
        }
    }
}
