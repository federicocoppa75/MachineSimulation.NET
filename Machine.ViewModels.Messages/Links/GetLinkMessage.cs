using Machine.ViewModels.Interfaces.Links;
using System;

namespace Machine.ViewModels.Messages.Links
{
    public struct GetLinkMessage
    {
        public int Id { get; set; } //= -1; // all link
        public Action<ILinkViewModel> SetLink { get; set; }
    }
}
