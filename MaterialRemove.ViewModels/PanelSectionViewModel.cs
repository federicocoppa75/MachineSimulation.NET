using g3;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels
{
    class PanelSectionViewModel : SectionElementViewModel, IPanelSection
    {
        static int _seedId;

        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double SizeZ { get; set; }
        public ISectionVolume Volume { get; set; }
        public IList<ISectionFace> Faces { get; set; }

        public PanelSectionViewModel() : base()
        {
            Id = _seedId++;
        }
    }
}
