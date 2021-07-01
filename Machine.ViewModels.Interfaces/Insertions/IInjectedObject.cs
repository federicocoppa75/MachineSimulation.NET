using Machine.Data.Base;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Insertions
{
    public interface IInjectedObject : IMachineElement
    {
        int InserterId { get; set; }
        public Point Position { get; set; }
        public Vector Direction { get; set; }
    }
}
