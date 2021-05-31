using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Interfaces.Models
{
    public interface ILazyAction
    {
        bool IsUpdated { get; }

        void Update();
    }
}
