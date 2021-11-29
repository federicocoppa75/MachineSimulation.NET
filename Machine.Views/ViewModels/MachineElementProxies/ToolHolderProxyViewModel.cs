using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using MDB = Machine.Data.Base;

namespace Machine.Views.ViewModels.MachineElementProxies
{
    [CategoryOrder("Tool holder", 2)]
    public class ToolHolderProxyViewModel : ElementProxyViewModel
    {
        private IToolholderElement ToolHolder => _element as IToolholderElement;

        [Category("Tool holder")]
        [PropertyOrder(0)]
        public ToolHolderType ToolHolderType => ToolHolder.ToolHolderType;

        [Category("Tool holder")]
        [PropertyOrder(1)]
        public int ToolHolderId 
        { 
            get => ToolHolder.ToolHolderId; 
            set => ToolHolder.ToolHolderId = value; 
        }

        [Category("Tool holder")]
        [PropertyOrder(2)]
        [ExpandableObject]
        public Vector Position 
        { 
            get => ToolHolder.Position.Convert(); 
            set => ToolHolder.Position = value.Convert(); 
        }

        [Category("Tool holder")]
        [PropertyOrder(3)]
        [ExpandableObject]
        public Vector Direction 
        { 
            get => ToolHolder.Direction.Convert(); 
            set => ToolHolder.Direction = value.ConvertV(); 
        }

        public ToolHolderProxyViewModel(IToolholderElement element) : base(element)
        {
        }
    }
}
