using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public class FlagViewModel : BaseViewModel, IFlag
    {
        public string Name { get; set; }

        private bool _value;
        public bool Value
        {
            get => _value;
            set => Set(ref _value, value, nameof(value));
        }


        public FlagViewModel() : base()
        {
        }

        public override string ToString() => Value.ToString();

        public bool TryToParse(string value)
        {
            bool result = false;

            if (string.Compare(value, "true", true) == 0)
            {
                Value = true;
                result = true;
            }
            else if (string.Compare(value, "false", true) == 0)
            {
                Value = false;
                result = true;
            }

            return result;
        }
    }
}
