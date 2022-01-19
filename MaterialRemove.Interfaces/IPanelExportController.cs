using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MaterialRemove.Interfaces
{
    public interface IPanelExportController
    {
        IPanel Panel { get; set; }

        ICommand ExportCommand { get; }
    }
}
