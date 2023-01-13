using Machine.Data.Base;
using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IColliderElement : IMachineElement
    {
        ColliderType Type { get; }
        double Radius { get; }
        ICollection<Point> Points { get; }
        Vector CollidingDirection { get; }
    }
}
