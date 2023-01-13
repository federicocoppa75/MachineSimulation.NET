using Machine.ViewModels.Interfaces.Helpers;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Factories
{
    public interface IColliderHelperFactory
    {
        IColliderHelper GetColliderHelper(IColliderElement collider, IPanelElement panel);

    }
}
