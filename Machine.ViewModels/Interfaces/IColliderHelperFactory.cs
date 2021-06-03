using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces
{
    public interface IColliderHelperFactory
    {
        IColliderHelper GetColliderHelper(ColliderElementViewModel collider, PanelViewModel panel);
    }
}
