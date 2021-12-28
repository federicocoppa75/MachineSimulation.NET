using Machine.Data.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels.ToolProxies
{
    internal static class ToolProxyHelper
    {
        public static ToolProxyViewModel Convert(this ITool tool)
        {
            if(tool is ICountersinkTool ct) return Convert(ct);
            else if(tool is IDiskOnConeTool doct) return Convert(doct);
            else if(tool is IDiskTool dt) return Convert(dt);
            else if(tool is IPointedTool pt) return Convert(pt);
            else if(tool is ISimpleTool st) return Convert(st);
            else if(tool is ITwoSectionTool tst) return Convert(tst);
            else throw new NotImplementedException();   
        }

        private static CountersinkProxyToolViewModel Convert(ICountersinkTool tool) => new CountersinkProxyToolViewModel(tool);

        private static DiskOnConeToolProxyViewModel Convert(IDiskOnConeTool tool) => new DiskOnConeToolProxyViewModel(tool);   

        private static DiskToolProxyViewModel Convert(IDiskTool tool) => new DiskToolProxyViewModel(tool);  

        private static PointedToolProxyViewModel Convert(IPointedTool tool) => new PointedToolProxyViewModel(tool);

        private static SimpleToolProxyViewModel Convert(ISimpleTool tool) => new SimpleToolProxyViewModel(tool);  

        private static TwoSectionToolProxyViewModel Convert(ITwoSectionTool tool) => new TwoSectionToolProxyViewModel(tool);
        
    }
}
