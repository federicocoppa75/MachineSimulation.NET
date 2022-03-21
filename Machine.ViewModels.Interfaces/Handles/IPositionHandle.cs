using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Handles
{
    public enum Type
    {
        X,
        Y,
        Z
    }

    public interface IPositionHandle
    {
        Type Type { get; }

        void StartMove();
        void Move(double stepX, double stepY, double stepZ);
    }   
}
