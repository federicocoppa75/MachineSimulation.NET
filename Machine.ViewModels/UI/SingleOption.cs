using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Machine.ViewModels.UI
{
    class SingleOption<T> : ISingleOption<T>, ISingleOption, INotifyPropertyChanged, INameProvider
    {
        private Func<string> _getName;

        private IValueProvider<T> _valueProvider;
        public IValueProvider<T> ValueProvider
        {
            get => _valueProvider;
            set
            {
                if (!ReferenceEquals(this, value))
                {
                    _valueProvider = value;

                    if (_valueProvider is INotifyPropertyChanged npc)
                    {
                        npc.PropertyChanged += (s, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
                    }
                }
            }
        }
        public T Value { get; set; }
        public bool Selected
        {
            get => ValueProvider.IsEqual(Value);
            set { if (value) ValueProvider.Value = Value; }
        }

        public string Name => _getName();

        public event PropertyChangedEventHandler PropertyChanged;

        public SingleOption()
        {
            _getName = () => Value.ToString();
        }

        public SingleOption(Func<string> getName)
        {
            _getName = getName;
        }
    }

}
