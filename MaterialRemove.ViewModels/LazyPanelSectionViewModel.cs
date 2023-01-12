using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Enums;
using MaterialRemove.ViewModels.Extensions;
using MaterialRemove.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels
{
    internal class LazyPanelSectionViewModel : PanelSectionViewModel, ILazyPanelSection, ISectionPositionProvider
    {
        class ThresholdPanelSection : IPanelSection
        {
            public int Id => throw new NotImplementedException();

            public double CenterX { get; set; }
            public double CenterY { get; set; }
            public double CenterZ { get; set; }
            public double SizeX { get; set; }
            public double SizeY { get; set; }
            public double SizeZ { get; set; }

            public ISectionVolume Volume => throw new NotImplementedException();

            public IList<ISectionFace> Faces => throw new NotImplementedException();
        }

        public int SectionsCountX { get; set; }
        public int SectionsCountY { get; set; }
        public SectionPosition SectionPosition { get; set; }

        public IList<IPanelSection> ListToUpdate { get; set; }

        private IPanelSection _thresholdToExplode;
        public IPanelSection ThresholdToExplode => _thresholdToExplode ?? (_thresholdToExplode = CreateThresholdToExplode());

        public LazyPanelSectionViewModel() : base()
        {
        }

        private IPanelSection CreateThresholdToExplode() => new ThresholdPanelSection()
        {
            CenterX = this.CenterX,
            CenterY = this.CenterY,
            CenterZ = this.CenterZ,
            SizeX = this.SizeX + 20,
            SizeY = this.SizeY + 20,
            SizeZ = this.SizeZ + 20,
        };

        public IList<IPanelSection> GetSubSections()
        {
            var division = new PanelExtensions.SectionDivision() { X = SectionsCountX, Y = SectionsCountY };
            var size = new PanelExtensions.SectionSize() { X = SizeX / SectionsCountX, Y = SizeY / SectionsCountY, Z = SizeZ };
            var center = new PanelExtensions.Position() { X = CenterX - SizeX / 2.0, Y = CenterY - SizeY / 2.0 };

            var sections = PanelExtensions.CreateSections(RemovalParameters,
                                                          this,
                                                          division,
                                                          size,
                                                          center,
                                                          CenterZ);

            return sections;
        }

        public SectionPosition GetSectionPosition(int nxSection, int nySection, int i, int j) => LazySectionExtension.GetSectionPosition(SectionPosition, nxSection, nySection, i, j);
    }
}
