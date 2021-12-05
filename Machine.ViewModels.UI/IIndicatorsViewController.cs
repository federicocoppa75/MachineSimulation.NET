using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface IIndicatorsViewController
    {
        bool Collider { get; set; }
        bool PanelHolder { get; set; }
        bool ToolHolder { get; set; }
        bool Inserter { get; set; }
    }
}
