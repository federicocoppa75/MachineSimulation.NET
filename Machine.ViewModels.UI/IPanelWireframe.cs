using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface IPanelWireframe
    { 
        bool Inner { get; set; }
        bool Outer { get; set; }
    }
}
