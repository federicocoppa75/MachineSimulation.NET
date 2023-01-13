using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.Interfaces.Helpers;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.MachineElements.Collider;

namespace Machine._3D.Views.Helpers
{
    class ColliderHelperFactory : IColliderHelperFactory
    {
        public IColliderHelper GetColliderHelper(IColliderElement collider, IPanelElement panel) => new ColliderHelper(collider, panel);
    }
}
