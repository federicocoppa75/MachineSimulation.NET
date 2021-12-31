using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Tools
{
    public interface IMeasurableTool
    {
        bool ProcessDimension(string propertyName, IToolDimension dimension);
    }
}
