using Machine.Data.Interfaces.Tools;
using Machine.ViewModels.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.ToolProxies
{
    internal class DiskToolProxyViewModel : ToolProxyViewModel, IMeasurableTool
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

        public DiskToolProxyViewModel() : base(CreateTool<IDiskTool>())
        {
            Name = $"Tool {++_newIdx}";
            Diameter = 120.0;
            CuttingThickness = 3.2;
            BodyThickness = 2.0;
            CuttingRadialThickness = 4.0;
            RadialUsefulLength = 35.0;
        }

        public DiskToolProxyViewModel(IDiskTool tool) : base(tool)
        {
        }

        protected override string GetToolType() => "Disk";

        #region IMeasurableTool
        public virtual bool ProcessDimension(string propertyName, IToolDimension dimension)
        {
            bool result = false;

            switch (propertyName)
            {
                case nameof(CuttingThickness):
                    result = ProcessLength(dimension, Diameter / 2.0, 10.0, -(CuttingThickness - BodyThickness) / 2.0, GetTotalLength() - (CuttingThickness - BodyThickness) / 2.0);
                    break;
                case nameof(BodyThickness):
                    result = ProcessLength(dimension, 10.0, Diameter / 2.0, 0.0, BodyThickness);
                    break;
                case nameof(Diameter):
                    result = ProcessDiameter(dimension, (CuttingThickness - BodyThickness) / 2.0, -10.0, Diameter);
                    break;
                case nameof(CuttingRadialThickness):
                    result = ProcessRadialDimension(dimension, Diameter / 2.0, -CuttingRadialThickness, (CuttingThickness - BodyThickness) / 2.0, -10.0);
                    break;
                case nameof(RadialUsefulLength):
                    result = ProcessRadialDimension(dimension, Diameter / 2.0, -RadialUsefulLength, (CuttingThickness - BodyThickness) / 2.0, -10.0);
                    break;
                default:
                    break;
            }

            if (result) dimension.PropertyName = propertyName;

            return result;
        }
        #endregion
    }
}
