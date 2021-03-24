using System;
using System.Collections.Generic;
using System.Text;
using MMT = MachineModels.Models.Tooling;
using MDT = Machine.Data.Toolings;
using System.IO;

namespace Client.Tooling.Helpers
{
    static class ModelToData
    {
        public static MDT.Tooling ToToolsData(this MMT.Tooling tooling)
        {
            var t = new MDT.Tooling() 
            { 
                Machine = Path.GetFileNameWithoutExtension(tooling.MachineFile), 
                Tools = Path.GetFileNameWithoutExtension(tooling.ToolsFile) 
            };

            foreach (var item in tooling.Units)
            {
                t.Units.Add(item.ToData());
            }

            return t;
        }

        private static MDT.ToolingUnit ToData(this MMT.ToolingUnit toolUnit)
        {
            return new MDT.ToolingUnit()
            {
                ToolHolderId = toolUnit.ToolHolderId,
                ToolName = toolUnit.ToolName
            };
        }
    }
}
