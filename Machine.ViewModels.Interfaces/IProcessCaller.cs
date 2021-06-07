using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces
{
    public interface IProcessCaller
    {
        bool Enable { get; set; }

        event EventHandler<DateTime> ProcessRequest;
    }
}
