using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Indicators
{
    public interface IPositionIndicator : IBaseIndicator
    {
        Point Position { get; set; }
    }
}
