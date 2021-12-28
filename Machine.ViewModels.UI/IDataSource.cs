using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Machine.ViewModels.UI
{
    public interface IDataSource : INameProvider
    {
        ICommand LoadMachineCommand { get; }
        ICommand SaveMachineCommand { get; }
        ICommand LoadToolingCommand { get; }
        ICommand SaveToolingCommand { get; }
        ICommand LoadToolsCommand { get; }
        ICommand SaveToolsCommand { get; }
        ICommand LoadEnvironmentCommand { get; }
        ICommand SaveEnvironmentCommand { get; }
    }
}
