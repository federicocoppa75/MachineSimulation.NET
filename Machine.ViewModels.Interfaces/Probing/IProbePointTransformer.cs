using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Probing
{
    public struct Point
    {
        public double X;
        public double Y;
        public double Z;
    }

    public interface IProbePointTransformer
    {
        Point Transform(Point point, bool gloablToLocal = false);
    }
}
