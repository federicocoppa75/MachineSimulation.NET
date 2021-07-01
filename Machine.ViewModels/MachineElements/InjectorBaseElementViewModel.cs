using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public abstract class InjectorBaseElementViewModel : ElementViewModel
    {
        public int InserterId { get; set; }
        public Point Position { get; set; }
        public Vector Direction { get; set; }
        public Color InserterColor { get; set; }
    }
}
