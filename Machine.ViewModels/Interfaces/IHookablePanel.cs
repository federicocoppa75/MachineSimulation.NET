using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces
{
    public interface IHookablePanel
    {
        void Hook(IPanelHooker hooker);
        void Unhook(IPanelHooker hooker);
    }
}
