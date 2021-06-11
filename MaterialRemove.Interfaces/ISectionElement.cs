using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.Interfaces
{
    public interface ISectionElement
    {
        int Id { get; }
        double CenterX { get; set; }
        double CenterY { get; set; }
        double CenterZ { get; set; }
        bool IsCorrupted { get; }
    }
}
