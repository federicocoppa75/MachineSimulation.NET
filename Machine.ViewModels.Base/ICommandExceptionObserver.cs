using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Base
{
    public interface ICommandExceptionObserver
    {
        void NotifyException(Exception exception);
    }
}
