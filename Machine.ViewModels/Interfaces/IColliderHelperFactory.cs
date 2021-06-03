using Machine.ViewModels.MachineElements;
using Machine.ViewModels.MachineElements.Collider;

namespace Machine.ViewModels.Interfaces
{
    public interface IColliderHelperFactory
    {
        IColliderHelper GetColliderHelper(ColliderElementViewModel collider, PanelViewModel panel);
    }
}
