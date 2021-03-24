using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels
{
    public class PropertyViewModel<T> : BaseViewModel where T : struct
    {
        public string Name { get; set; }

        private T _value;
        public T Value
        {
            get => _value; 
            set => Set(ref _value, value, nameof(Value)); 
        }

        public Type PropertyType => typeof(T);

        public PropertyViewModel() : base()
        {

        }
    }
}
