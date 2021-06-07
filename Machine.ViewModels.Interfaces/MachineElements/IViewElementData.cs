using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IViewElementData
    {
        bool IsVisible { get; set; }
        bool IsSelected { get; set; }
        string PostEffects { get; set; }
        bool IsExpanded { get; set; }
    }
}
