using Machine.Data.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Bridge
{
    public interface IToolDataProxy
    {
        ITool Tool { get; }
    }
}
