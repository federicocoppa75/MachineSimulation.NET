using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces
{
    public interface IMovablePanel
    {
        double OffsetX { get; set; }

        event EventHandler<double> ValueChanged;
    }
}
