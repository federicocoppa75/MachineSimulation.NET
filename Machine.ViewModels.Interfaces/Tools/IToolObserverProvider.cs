using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Tools
{
    public interface IToolObserverProvider
    {
        IToolsObserver Observer { get; }
    }
}
