using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.MachineElementProxies
{
    [CategoryOrder("Collider", 2)]
    public class ColliderProxyViewModel : ElementProxyViewModel
    {
        [Category("Collider")]
        public ColliderType ColliderType => (_element as IColliderElement).Type;

        public ColliderProxyViewModel(IColliderElement element) : base(element)
        {
        }
    }
}
