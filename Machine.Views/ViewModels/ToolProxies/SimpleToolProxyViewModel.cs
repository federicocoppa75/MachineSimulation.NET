using Machine.Data.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.ToolProxies
{
    internal class SimpleToolProxyViewModel : ToolProxyViewModel, ISimpleTool
    {
        private ISimpleTool Tool => GetTool<ISimpleTool>();

        [Category("Geometry")]
        [PropertyOrder(1)]
        public double Diameter 
        { 
            get => Tool.Diameter;
            set
            {
                if(Set(Tool.Diameter, value, v => Tool.Diameter = v, nameof(Diameter))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(2)]
        public double Length 
        {
            get => Tool.Length; 
            set
            {
                if(Set(Tool.Length, value, v => Tool.Length = v, nameof(Length))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(3)]
        public double UsefulLength 
        { 
            get => Tool.UsefulLength; 
            set
            {
                if(Set(Tool.UsefulLength, value, v => Tool.UsefulLength = v, nameof(UsefulLength))) UpdateTool();
            }
        }

        public SimpleToolProxyViewModel(ISimpleTool tool) : base(tool)
        {

        }
    }
}
