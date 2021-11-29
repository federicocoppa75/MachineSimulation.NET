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
    [CategoryOrder("Inserter", 3)]
    public class InserterProxyViewModel : InjectorProxyViewModel
    {
        private IInserterElement Inserter => _element as IInserterElement;

        [Category("Inserter")]
        [PropertyOrder(0)]
        public double Diameter 
        { 
            get => Inserter.Diameter; 
            set => Inserter.Diameter = value; 
        }

        [Category("Inserter")]
        [PropertyOrder(1)]
        public double Length 
        { 
            get => Inserter.Length; 
            set => Inserter.Length = value; 
        }

        [Category("Inserter")]
        [PropertyOrder(2)]
        public int LoaderLinkId
        { 
            get => Inserter.LoaderLinkId; 
            set => Inserter.LoaderLinkId = value; 
        }

        [Category("Inserter")]
        [PropertyOrder(3)]
        public int DischargerLinkId 
        { 
            get => Inserter.DischargerLinkId; 
            set => Inserter.DischargerLinkId = value; 
        }

        public InserterProxyViewModel(IInserterElement element) : base(element)
        {

        }
    }
}
