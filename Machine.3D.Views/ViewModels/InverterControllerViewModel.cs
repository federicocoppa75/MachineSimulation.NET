using Machine.ViewModels.Base;
using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.ViewModels
{
    internal class InverterControllerViewModel : BaseViewModel, IInvertersController
    {
        private string _name;

        public string Name
        {
            get => _name; 
            set => Set(ref _name, value, nameof(Name)); 
        }

        private int _value;

        public int Value
        {
            get => _value; 
            set => Set(ref _value, value, nameof(Value));
        }

        private bool _isVisible;

        public bool IsVisible
        {
            get => _isVisible; 
            set => Set(ref _isVisible, value, nameof(IsVisible));
        }


        public void TurnOff()
        {
            Name = string.Empty;
            Value = 0;
            IsVisible = false;
        }

        public void TurnOn(string name, int value)
        {
            Name = name;
            Value = value;
            IsVisible = true;
        }

        public void Change(int value)
        {
            Value = value;
        }
    }
}
