using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Messages.Links.Gantry
{
    public struct GantryMessage
    {
        public bool State { get; set; }
        public int Master { get; set; }
        public int Slave { get; set; }
    }
}
