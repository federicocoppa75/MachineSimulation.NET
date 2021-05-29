using Machine.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Machine.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private IMessenger _messenger;

        protected IMessenger Messenger
        {
            get 
            {
                if(_messenger == null)
                {
                    _messenger = Ioc.SimpleIoc<IMessenger>.GetInstance();
                }

                return _messenger; 
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void RisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T property, T value, string propertyName)
        {
            bool result = false;

            if (!Comparer<T>.Instance.IsEqual(property, value))
            {
                property = value;
                result = true;

                RisePropertyChanged(propertyName);
            }

            return result;
        }

        protected T GetInstance<T>() => Ioc.SimpleIoc<T>.GetInstance();
    }
}
