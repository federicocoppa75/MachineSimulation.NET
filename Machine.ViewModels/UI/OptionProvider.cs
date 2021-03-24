using System.Collections.Generic;
using System.ComponentModel;

namespace Machine.ViewModels.UI
{
    public abstract class OptionProvider<T> : IOptionProvider, IValueProvider<T>, INotifyPropertyChanged
    {
        public string Name { get; set; }

        public abstract IEnumerable<ISingleOption> Options { get; }

        private T _value;

        public T Value
        {
            get => _value; 
            set
            {
                if(!IsEqual(value))
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract bool IsEqual(T value);

        public override string ToString() => Value.ToString();

        public abstract bool TryToParse(string value);
    }
}
