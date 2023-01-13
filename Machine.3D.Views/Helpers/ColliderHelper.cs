using Machine.ViewModels.Interfaces.Helpers;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.MachineElements.Collider;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Machine._3D.Views.Helpers
{
    class ColliderHelper : BaseColliderHelper, IColliderHelper
    {
        IColliderElement _collider;
        IPanelElement _panel;

        public ColliderHelper(IColliderElement collider, IPanelElement panel)
        {
            _collider = collider;
            _panel = panel;
        }

        protected override Point3D[] GetColliderPoints() => _collider.Points.Select(p => new Point3D(p.X, p.Y, p.Z)).ToArray();
        protected override Vector3D GetColliderDirection() => new Vector3D(_collider.CollidingDirection.X, _collider.CollidingDirection.Y, _collider.CollidingDirection.Z);
        protected override Size3D GetPanelSize() => new Size3D(_panel.SizeX, _panel.SizeY, _panel.SizeZ);
        protected override Matrix3D GetColliderChainTransformation() => _collider.GetChainTransformation();
        protected override Matrix3D GetPanelChainTransformation() => _panel.GetChainTransformation();
    }
}
