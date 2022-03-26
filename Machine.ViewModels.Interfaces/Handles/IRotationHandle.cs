using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Handles
{
    public interface IRotationHandle
    {
        Type Type { get; }

        void StartRotate();

        void Rotate(double angle);
    }
}
