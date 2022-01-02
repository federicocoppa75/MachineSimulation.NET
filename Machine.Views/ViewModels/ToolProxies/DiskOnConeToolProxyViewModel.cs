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
    internal class DiskOnConeToolProxyViewModel : DiskToolProxyViewModel, IMeasurableTool
    {
        private IDiskOnConeTool Tool => GetTool<IDiskOnConeTool>();

        [Category("Postponent")]
        [PropertyOrder(2)]
        public double PostponemntDiameter 
        {
            get => Tool.PostponemntDiameter;
            set
            {
                if(Set(Tool.PostponemntDiameter, value, v => Tool.PostponemntDiameter = v, nameof(PostponemntDiameter))) UpdateTool();
            }
        }

        [Category("Postponent")]
        [PropertyOrder(1)]
        public double PostponemntLength
        {
            get => Tool.PostponemntLength;
            set
            {
                if (Set(Tool.PostponemntLength, value, v => Tool.PostponemntLength = v, nameof(PostponemntLength))) UpdateTool();
            }
        }

        public DiskOnConeToolProxyViewModel() : base(CreateTool<IDiskOnConeTool>())
        {
            Name = $"Tool {++_newIdx}";
            PostponemntDiameter = 16.0;
            PostponemntLength = 120.5;
            Diameter = 100.0;
            CuttingRadialThickness = 2.0;
            BodyThickness = 4.0;
            CuttingThickness = 6.6;
            RadialUsefulLength = 35.0;
        }

        public DiskOnConeToolProxyViewModel(IDiskOnConeTool tool) : base(tool)
        {
        }

        protected override string GetToolType() => "DiskOnCone";

        #region IMeasurableTool
        public override bool ProcessDimension(string propertyName, IToolDimension dimension)
        {
            bool result = false;

            switch (propertyName)
            {
                case nameof(CuttingThickness):
                    result = ProcessLength(dimension, Diameter / 2.0, 10.0, PostponemntLength - (CuttingThickness - BodyThickness) / 2.0, GetTotalLength());
                    break;
                case nameof(BodyThickness):
                    result = ProcessLength(dimension, 10.0, Diameter / 2.0, PostponemntLength, PostponemntLength + BodyThickness);
                    break;
                case nameof(Diameter):
                    result = ProcessDiameter(dimension, GetTotalLength() - (CuttingThickness - BodyThickness) / 2.0, 10.0, Diameter);
                    break;
                case nameof(CuttingRadialThickness):
                    result = ProcessRadialDimension(dimension, Diameter / 2.0, -CuttingRadialThickness, PostponemntLength - (CuttingThickness - BodyThickness) / 2.0, -10.0);
                    break;
                case nameof(RadialUsefulLength):
                    result = ProcessRadialDimension(dimension, Diameter / 2.0, -RadialUsefulLength, PostponemntLength - (CuttingThickness - BodyThickness) / 2.0, -10.0);
                    break;
                case nameof(PostponemntLength):
                    result = ProcessLength(dimension, PostponemntDiameter / 2.0, 30.0, 0.0, PostponemntLength);
                    break;
                case nameof(PostponemntDiameter):
                    result = ProcessDiameter(dimension, PostponemntLength - 10.0, 10.0, PostponemntDiameter);
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
