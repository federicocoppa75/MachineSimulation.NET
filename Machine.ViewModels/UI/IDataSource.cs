using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Machine.ViewModels.UI
{
    public interface IDataSource : INameProvider
    {
        ICommand LoadMachineCommand { get; }
        ICommand LoadToolingCommand { get; }
    }
}
