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
    internal class SimpleToolProxyViewModel : ToolProxyViewModel, IMeasurableTool
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

        public SimpleToolProxyViewModel() : base(CreateTool<ISimpleTool>())
        {
            Name = $"Tool {++_newIdx}";
            Diameter = 10.0;
            Length = 50.0;
            UsefulLength = 30.0;
        }

        public SimpleToolProxyViewModel(ISimpleTool tool) : base(tool)
        {
        }

        public SimpleToolProxyViewModel(SimpleToolProxyViewModel src) : base(CreateTool<ISimpleTool>())
        {
            Diameter = src.Diameter;
            Description = src.Description;
            ToolLinkType = src.ToolLinkType;
            ConeModelFile = src.ConeModelFile;
            Length = src.Length;
            UsefulLength = src.UsefulLength;
            Name = $"{src.Name} (copy)";
        }

        public override ToolProxyViewModel CreateCopy() => new SimpleToolProxyViewModel(this);

        protected override string GetToolType() => "Simple";

        #region IMeasurableTool
        public bool ProcessDimension(string propertyName, IToolDimension dimension)
        {
            bool result = false;
            double d = (!string.IsNullOrEmpty(ConeModelFile) || (Diameter > 60.0)) ? 40 - Diameter / 2.0 : 10;

            switch (propertyName)
            {
                case nameof(Length):
                    result = ProcessLength(dimension, Diameter / 2.0, d, 0.0, Length);
                    break;
                case nameof(UsefulLength):
                    result = ProcessLength(dimension, Diameter / 2.0, 10.0, Length - UsefulLength, Length);
                    break;
                case nameof(Diameter):
                    result = ProcessDiameter(dimension, Length, 10.0, Diameter);
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
