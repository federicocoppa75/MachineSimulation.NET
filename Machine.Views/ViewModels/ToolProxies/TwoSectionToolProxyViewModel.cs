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
    internal class TwoSectionToolProxyViewModel : ToolProxyViewModel, IMeasurableTool
    {
        private ITwoSectionTool Tool => GetTool<ITwoSectionTool>();

        [Category("Geometry")]
        [PropertyOrder(1)]
        public double Diameter1 
        {
            get => Tool.Diameter1;
            set
            {
                if(Set(Tool.Diameter1, value, v => Tool.Diameter1 = v, nameof(Diameter1))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(2)]
        public double Diameter2 
        { 
            get => Tool.Diameter2; 
            set
            {
                if(Set(Tool.Diameter2, value, v => Tool.Diameter2 = v, nameof(Diameter2))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(3)]
        public double Length1 
        {
            get => Tool.Length1;
            set
            {
                if(Set(Tool.Length1, value, v => Tool.Length1 = v, nameof(Length1))) UpdateTool();  
            }            
        }

        [Category("Geometry")]
        [PropertyOrder(4)]
        public double Length2
        {
            get => Tool.Length2;
            set
            {
                if (Set(Tool.Length2, value, v => Tool.Length2 = v, nameof(Length2))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(5)]
        public double UsefulLength
        {
            get => Tool.UsefulLength;
            set
            {
                if(Set(Tool.UsefulLength, value, v => Tool.UsefulLength = v, nameof(UsefulLength))) UpdateTool();
            }
        }

        public TwoSectionToolProxyViewModel() : base(CreateTool<ITwoSectionTool>())
        {
            Name = $"Tool {++_newIdx}";
            Diameter1 = 10.0;
            Diameter2 = 35.0;
            Length1 = 25.0;
            Length2 = 10.0;
            UsefulLength = 10.0;
        }

        public TwoSectionToolProxyViewModel(ITwoSectionTool tool) : base(tool)
        {
        }

        protected override string GetToolType() => "TwoSection";

        #region IMeasurableTool
        public bool ProcessDimension(string propertyName, IToolDimension dimension)
        {
            bool result = false;

            switch (propertyName)
            {
                case nameof(Length1):
                    result = ProcessLength(dimension, Diameter1 / 2.0, Diameter2 / 2.0 + 10.0, 0.0, Length1);
                    break;
                case nameof(Length2):
                    result = ProcessLength(dimension, Diameter2 / 2.0, 10.0, Length1, GetTotalLength());
                    break;
                case nameof(UsefulLength):
                    result = ProcessLength(dimension, 0.0, Diameter2 / 2.0 + 10.0, GetTotalLength() - UsefulLength, GetTotalLength());
                    break;
                case nameof(Diameter1):
                    result = ProcessDiameter(dimension, 0.0, 10.0, Diameter1);
                    break;
                case nameof(Diameter2):
                    result = ProcessDiameter(dimension, GetTotalLength(), 10.0, Diameter2);
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
