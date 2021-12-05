using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Indicators
{
    public interface IPositionAndDirectionIndicator : IPositionIndicator
    {
        Vector Direction { get; set; }
    }
}
