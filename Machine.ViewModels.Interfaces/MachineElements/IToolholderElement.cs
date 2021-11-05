using Machine.Data.Base;
using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IToolholderElement : IToolholderBase
    {
        int ToolHolderId { get; }
        ToolHolderType ToolHolderType { get; }
        bool ActiveTool { get; set; }
    }
}
