using Machine.ViewModels.Messaging;
using System;
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

        protected T GetInstance<T>() => Ioc.SimpleIoc<T>.GetInstance();
        protected bool HasInstance<T>() => Ioc.SimpleIoc<T>.HasInstance();

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
