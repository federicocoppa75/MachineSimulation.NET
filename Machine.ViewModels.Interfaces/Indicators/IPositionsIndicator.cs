using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Indicators
{
    public interface IPositionsIndicator : IBaseIndicator
    {
        ICollection<Point> Points { get; }
    }
}
