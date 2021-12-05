using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Indicators
{
    public interface IIndicatorProxy
    {
        bool TryGetIndicator<T>(out T indicator) where T : IBaseIndicator;
    }
}
