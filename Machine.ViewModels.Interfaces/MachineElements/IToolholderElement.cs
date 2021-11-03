using Machine.Data.Base;
using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.MachineElements
{
    public interface IToolholderElement
    {
        int ToolHolderId { get; }
        ToolHolderType ToolHolderType { get; }
        Point Position { get; set; }
        Vector Direction { get; set; }
        bool ActiveTool { get; set; }
    }
}
