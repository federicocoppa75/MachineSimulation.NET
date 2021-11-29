using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IColliderElement : IMachineElement
    {
        ColliderType Type { get; }
    }
}
