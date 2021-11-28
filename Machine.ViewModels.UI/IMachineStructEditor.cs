using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Machine.ViewModels.UI
{
    public interface IAddElementCommand
    {
        string Label { get; }
        ICommand Command { get; }
    }

    public interface IMachineStructEditor
    {
        IEnumerable<IAddElementCommand> AddCommands { get; }
        ICommand DeleteCommand { get; }
    }
}
