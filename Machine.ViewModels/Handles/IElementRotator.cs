using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Handles
{
    public interface IElementRotator
    {
        void Rotate(double x, double y, double z, double angle);
    }
}
