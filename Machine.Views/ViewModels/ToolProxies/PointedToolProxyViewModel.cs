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
    internal class PointedToolProxyViewModel : ToolProxyViewModel
    {
        private IPointedTool Tool => GetTool<IPointedTool>();

        [Category("Geometry")]
        [PropertyOrder(3)]
        public double ConeHeight 
        { 
            get => Tool.ConeHeight; 
            set
            {
                if(Set(Tool.ConeHeight, value, v => Tool.ConeHeight = v, nameof(ConeHeight))) UpdateTool();
            }
        }

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
        public double StraightLength 
        { 
            get => Tool.StraightLength;
            set 
            {
                if(Set(Tool.StraightLength, value, v => Tool.StraightLength = v, nameof(StraightLength))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(4)]
        public double UsefulLength 
        { 
            get => Tool.UsefulLength; 
            set
            {
                if(Set(Tool.UsefulLength, value, v => Tool.UsefulLength = v, nameof(UsefulLength))) UpdateTool();
            }
        }

        public PointedToolProxyViewModel() : base(CreateTool<IPointedTool>())
        {
            Name = $"Tool {++_newIdx}";
            Diameter = 10.0;
            StraightLength = 40.0;
            ConeHeight = 10.0;
            UsefulLength = 40.0;
        }

        public PointedToolProxyViewModel(IPointedTool tool) : base(tool)
        {
        }

        protected override string GetToolType() => "Pointed";
    }
}
