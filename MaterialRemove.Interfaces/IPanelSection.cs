using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.Interfaces
{
    public interface IPanelSection
    {
        int Id { get; }
        double CenterX { get; set; }
        double CenterY { get; set; }
        double CenterZ { get; set; }
        double SizeX { get; set; }
        double SizeY { get; set; }
        double SizeZ { get; set; }
        ISectionVolume Volume { get; }
        IList<ISectionFace> Faces { get; }
    }
}
