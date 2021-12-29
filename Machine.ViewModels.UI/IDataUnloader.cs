using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Machine.ViewModels.UI
{
    public interface IDataUnloader
    {
        ICommand UnloadCommand { get; }
    }
}
