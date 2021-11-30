using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.Messages.Links
{
    class AddedLinkMessage
    {
        public ILinkViewModel Link { get; set; }
    }
}
