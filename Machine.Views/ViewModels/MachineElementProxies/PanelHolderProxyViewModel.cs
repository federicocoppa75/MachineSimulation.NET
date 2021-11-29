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
    [CategoryOrder("Panel holder", 2)]
    public class PanelHolderProxyViewModel : ElementProxyViewModel
    {
        private IPanelholderElement PanelHolder => _element as IPanelholderElement;

        [Category("Panel holder")]
        [PropertyOrder(0)]
        public int PanelHolderId 
        { 
            get => PanelHolder.PanelHolderId; 
            set => PanelHolder.PanelHolderId = value; 
        }

        [Category("Panel holder")]
        [PropertyOrder(1)]
        public string PanelHolderName 
        { 
            get => PanelHolder.PanelHolderName;
            set => PanelHolder.PanelHolderName = value; 
        }

        [Category("Panel holder")]
        [PropertyOrder(2)]
        [ExpandableObject]
        public Vector Position 
        { 
            get => PanelHolder.Position.Convert(); 
            set => PanelHolder.Position = value.Convert(); 
        }

        [Category("Panel holder")]
        [PropertyOrder(3)]
        public PanelLoadType Corner 
        { 
            get => PanelHolder.Corner; 
            set => PanelHolder.Corner = value; 
        }

        public PanelHolderProxyViewModel(IPanelholderElement element) : base(element)
        {
        }
    }
}
