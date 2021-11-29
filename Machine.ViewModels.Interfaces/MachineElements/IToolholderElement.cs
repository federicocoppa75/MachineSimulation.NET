using Machine.Data.Base;
using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IToolholderElement : IToolholderBase, IMachineElement
    {
        int ToolHolderId { get; set; }
        ToolHolderType ToolHolderType { get; }
        bool ActiveTool { get; set; }
    }
}
