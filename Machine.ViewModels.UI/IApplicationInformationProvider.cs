using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public enum ApplicationType
    {
        MachineViewer,
        MachineEditor,
        ToolEditor,
        ToolingEditor
    }

    public interface IApplicationInformationProvider
    {
        ApplicationType ApplicationType { get; }
    }
}
