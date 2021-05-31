using Machine.Data.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class ATToolholderViewModel : ElementViewModel
    {
        public Point Position { get; set; }
        public Vector Direction { get; set; }

        public ATToolholderViewModel() : base()
        {
        }
    }
}
