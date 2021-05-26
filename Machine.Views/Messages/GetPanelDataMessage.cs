using Machine.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.Messages
{
    class GetPanelDataMessage
    {
        public Action<PanelData> SetPanelData { get; set; }
    }
}
