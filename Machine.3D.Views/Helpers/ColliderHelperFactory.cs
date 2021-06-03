using Machine.ViewModels.Interfaces;
using Machine.ViewModels.MachineElements;

namespace Machine._3D.Views.Helpers
{
    class ColliderHelperFactory : IColliderHelperFactory
    {
        public IColliderHelper GetColliderHelper(ColliderElementViewModel collider, PanelViewModel panel) => new ColliderHelper(collider, panel);
    }
}
