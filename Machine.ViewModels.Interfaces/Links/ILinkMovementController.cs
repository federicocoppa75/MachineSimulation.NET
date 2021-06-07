using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Links
{
    public interface ILinkMovementController
    {
        int MinTimespam { get; set; }
        bool Enable { get; set; }
    }
}
