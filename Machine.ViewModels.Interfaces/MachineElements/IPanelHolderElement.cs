using Machine.Data.Base;
using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IPanelholderElement : IMachineElement
    {
        int PanelHolderId { get; set; }
        string PanelHolderName { get; set; }
        Point Position { get; set; }
        PanelLoadType Corner { get; set; }
        IPanelElement LoadedPanel { get; set; }
    }
}
