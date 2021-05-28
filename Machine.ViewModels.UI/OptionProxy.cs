using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Machine.ViewModels.UI
{
    public abstract class OptionProxy<T> : IOptionProvider<T>, IValueProvider<T>, INotifyPropertyChanged
    {
        private readonly Func<IEnumerable<T>> _getOptions;
        private readonly Func<T> _getValue;
        private readonly Action<T> _setValue;

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<T> Options => _getOptions();

        public T Value 
        {
            get => _getValue();
            set
            {
                if (!IsEqual(value))
                {
                    _setValue(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public IEnumerable<ISingleOption<T>> OptionFlags => _getOptions().Select(o => new SingleOption<T>() { ValueProvider = this, Value = o });

        public OptionProxy(Func<IEnumerable<T>> getOptions, Func<T> getValue, Action<T> setValue)
        {
            _getOptions = getOptions;
            _getValue = getValue;
            _setValue = setValue;
        }

        //public bool IsEqual(T value) => EqualityComparer<T>.Default.Equals(Value, value);

        //public bool TryToParse(string value)
        //{
        //    var result = false;

        //    if (Enum.TryParse<T>(value, out T v))
        //    {
        //        Value = v;
        //        result = true;
        //    }

        //    return result;
        //}

        public abstract bool IsEqual(T value);

        public abstract bool TryToParse(string value);
    }
}
