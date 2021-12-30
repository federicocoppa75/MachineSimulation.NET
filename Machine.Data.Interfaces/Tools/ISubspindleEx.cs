using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Interfaces.Tools
{
    public interface ISubspindleEx : ISubspindle
    {
        ITool GetTool();
        void SetTool(ITool tool);
    }
}
