using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces
{
    public interface IColliderHelper
    {
        bool Intersect(out double distance);
    }
}
