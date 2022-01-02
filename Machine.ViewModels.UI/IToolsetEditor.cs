using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Machine.ViewModels.UI
{
    public interface IAddToolCommand
    {
        string Label { get; }
        ICommand Command { get; }
    }

    public interface IToolsetEditor
    {
        IEnumerable<IAddToolCommand> AddCommands { get; }
        ICommand AddCopyCommand { get; }
        ICommand DeleteCommand { get; }
    }
}
