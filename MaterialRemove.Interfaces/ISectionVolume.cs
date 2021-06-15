using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.Interfaces
{
    public interface ISectionVolume : ISectionElement
    {
        double SizeX { get; set; }
        double SizeY { get; set; }
        double SizeZ { get; set; }
    }
}
