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
    internal class DiskToolProxyViewModel : ToolProxyViewModel, IDiskTool
    {
        private IDiskTool Tool => GetTool<IDiskTool>();

        [Category("Geometry")]
        [PropertyOrder(3)]
        public double BodyThickness 
        {
            get => Tool.BodyThickness;
            set
            {
                if (Set(Tool.BodyThickness, value, v => Tool.BodyThickness = v, nameof(BodyThickness))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(4)]
        public double CuttingRadialThickness 
        { 
            get => Tool.CuttingRadialThickness; 
            set
            {
                if (Set(Tool.CuttingRadialThickness, value, v => Tool.CuttingRadialThickness = v, nameof(CuttingRadialThickness))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(2)]
        public double CuttingThickness 
        { 
            get => Tool.CuttingThickness; 
            set
            {
                if (Set(Tool.CuttingThickness, value, v => Tool.CuttingThickness = v, nameof(CuttingThickness))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(1)]
        public double Diameter 
        { 
            get => Tool.Diameter; 
            set
            {
                if (Set(Tool.Diameter, value, v => Tool.Diameter = v, nameof(Diameter))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(5)]
        public double RadialUsefulLength 
        { 
            get => Tool.RadialUsefulLength; 
            set
            {
                if (Set(Tool.RadialUsefulLength, value, v => Tool.RadialUsefulLength = v, nameof(RadialUsefulLength))) UpdateTool();
            }
        }

        public DiskToolProxyViewModel(IDiskTool tool) : base(tool)
        {

        }
    }
}
