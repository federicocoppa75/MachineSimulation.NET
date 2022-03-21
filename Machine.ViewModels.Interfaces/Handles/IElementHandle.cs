using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Handles
{
    public interface IElementHandle
    {
        public double MinX { get; }
        public double MinY { get; }
        public double MinZ { get; }
        public double MaxX { get; }
        public double MaxY { get; }
        public double MaxZ { get; }

    }
}
