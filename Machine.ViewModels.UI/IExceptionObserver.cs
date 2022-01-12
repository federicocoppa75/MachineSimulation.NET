using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface IExceptionObserver
    {
        void NotifyException(Exception exception);
    }
}
