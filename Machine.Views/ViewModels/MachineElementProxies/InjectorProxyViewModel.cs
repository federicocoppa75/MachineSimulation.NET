using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.MachineElementProxies
{
    [CategoryOrder("Injector", 2)]
    public class InjectorProxyViewModel : ElementProxyViewModel
    {
        protected IInjectorElement Injector => _element as IInjectorElement;

        [Category("Injector")]
        [PropertyOrder(0)]
        public int InserterId 
        { 
            get => Injector.InserterId; 
            set => Injector.InserterId = value; 
        }

        [Category("Injector")]
        [PropertyOrder(1)]
        [ExpandableObject]
        public Vector Position 
        {
            get => Injector.Position.Convert(); 
            set => Injector.Position = value.Convert(); 
        }

        [Category("Injector")]
        [PropertyOrder(2)]
        [ExpandableObject]
        public Vector Direction 
        { 
            get => Injector.Direction.Convert(); 
            set => Injector.Direction = value.ConvertV(); 
        }

        [Category("Injector")]
        [PropertyOrder(3)]
        public Color InserterColor 
        { 
            get => Injector.Color.Convert(); 
            set => Injector.Color = value.Convert(); 
        }

        public InjectorProxyViewModel(IInjectorElement element) : base(element)
        {
        }
    }
}
