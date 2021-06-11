using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.Interfaces
{
    public interface ISectionVolume : ISectionElement
    {
        //int Id { get; }
        //double CenterX { get; set; }
        //double CenterY { get; set; }
        //double CenterZ { get; set; }
        double SizeX { get; set; }
        double SizeY { get; set; }
        double SizeZ { get; set; }
    }
}
