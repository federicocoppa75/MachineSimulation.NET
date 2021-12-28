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
    internal class CountersinkProxyToolViewModel : ToolProxyViewModel, ICountersinkTool
    {
        private ICountersinkTool Tool => GetTool<ICountersinkTool>();

        [Category("Geometry")]
        [PropertyOrder(1)]
        public double Diameter1
        {
            get => Tool.Diameter1;
            set
            {
                if (Set(Tool.Diameter1, value, v => Tool.Diameter1 = v, nameof(Diameter1))) UpdateTool();
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
                if (Set(Tool.Length1, value, v => Tool.Length1 = v, nameof(Length1))) UpdateTool();
            }
        }

        [Category("Geometry")]
        [PropertyOrder(4)]
        public double Length2
        {
            get => Tool.Length2;
            set
            {
                if(Set(Tool.Length2, value, v => Tool.Length2 = v, nameof(Length2)))
                {
                    UpdateTool();
                }
            }
        }

        [Category("Geometry")]
        [PropertyOrder(5)]
        public double Length3
        {
            get => Tool.Length3;
            set
            {
                if (Set(Tool.Length3, value, v => Tool.Length3 = v, nameof(Length3)))
                {
                    UpdateTool();
                }
            }
        }

        [Category("Geometry")]
        [PropertyOrder(6)]
        public double UsefulLength 
        { 
            get => Tool.UsefulLength; 
            set 
            { 
                if(Set(Tool.UsefulLength, value, v => Tool.UsefulLength = v, nameof(UsefulLength))) UpdateTool();
            } 
        }


        public CountersinkProxyToolViewModel(ICountersinkTool tool) : base(tool)
        {
        }
    }
}
