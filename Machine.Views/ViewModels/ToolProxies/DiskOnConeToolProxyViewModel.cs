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
    internal class DiskOnConeToolProxyViewModel : DiskToolProxyViewModel, IDiskOnConeTool
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

        public DiskOnConeToolProxyViewModel(IDiskOnConeTool tool) : base(tool)
        {

        }
    }
}
