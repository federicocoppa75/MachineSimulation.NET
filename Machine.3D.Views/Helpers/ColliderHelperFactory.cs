using Machine.ViewModels.Interfaces;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.MachineElements.Collider;

namespace Machine._3D.Views.Helpers
{
    class ColliderHelperFactory : IColliderHelperFactory
    {
        public IColliderHelper GetColliderHelper(ColliderElementViewModel collider, PanelViewModel panel) => new ColliderHelper(collider, panel);
    }
}
