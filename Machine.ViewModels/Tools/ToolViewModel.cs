using Machine.Data.Enums;
using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Tools
{
    public class ToolViewModel : BaseViewModel
    {
        public int ToolID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ToolLinkType ToolLinkType { get; set; }
        public string ConeModelFile { get; set; }
    }
}
