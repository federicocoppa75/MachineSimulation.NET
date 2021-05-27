using Machine.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages.Links
{
    public class GetLinkMessage
    {
        public int Id { get; set; } = -1; // all link
        public Action<LinkViewModel> SetLink { get; set; }
    }
}
