using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.Interfaces.Helpers;
using Machine.ViewModels.Interfaces.MachineElements;

namespace Machine._3D.Views.Helpers
{
    class ColliderHelperFactory : IColliderHelperFactory
    {
        public IColliderHelper GetColliderHelper(IColliderElement collider, IPanelElement panel) => new ColliderHelper(collider, panel);
    }
}
