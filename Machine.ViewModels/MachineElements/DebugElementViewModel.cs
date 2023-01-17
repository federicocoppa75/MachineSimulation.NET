using Machine.Data.Base;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class DebugElementViewModel : ElementViewModel, IDebugElementViewModel
    {
        private static int _seedId = 0;

        public DebugElementViewModel(double x, double y, double z) : base()
        {
            Transformation = new Matrix() { M11 = 1.0, M22 = 1.0, M33 = 1.0, OffsetX = x, OffsetY = y, OffsetZ = z };
            Name = $"debug {_seedId++}";
        }
    }
}
