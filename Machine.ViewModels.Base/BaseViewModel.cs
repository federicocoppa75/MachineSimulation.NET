using Machine.ViewModels.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Machine.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
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

        protected bool Set<T>(T propertyValue, T value, Action<T> setAction, string propertyName)
        {
            bool result = false;

            if (!Comparer<T>.Instance.IsEqual(propertyValue, value))
            {
                setAction(value);
                result = true;

                RisePropertyChanged(propertyName);
            }

            return result;
        }

        protected T GetInstance<T>() => Ioc.SimpleIoc<T>.GetInstance();
        protected T GetInstance<T>(string key) => Ioc.SimpleIoc<T>.GetInstance(key);
        protected bool HasInstance<T>() => Ioc.SimpleIoc<T>.HasInstance();
        protected IEnumerable<T> GetInstances<T>() => Ioc.SimpleIoc<T>.GetInstances();

        #region IDisposable
        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing) { }

            _disposed = true;
        }

        #endregion
    }
}
