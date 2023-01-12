using g3;
using MaterialRemove.Interfaces;
using MaterialRemove.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVMIoc = Machine.ViewModels.Ioc;
using MRVMI = MaterialRemove.ViewModels.Interfaces;
using System.Collections.ObjectModel;
using MaterialRemove.ViewModels.Interfaces;
using static System.Collections.Specialized.BitVector32;

namespace MaterialRemove.ViewModels.Extensions
{
    public static class PanelExtensions
    {
        class SectionPositionProvider : ISectionPositionProvider
        {
            public SectionPosition GetSectionPosition(int nxSection, int nySection, int i, int j) => PanelExtensions.GetSectionPosition(nxSection, nySection, i, j);
        }

        static ISectionPositionProvider _sectonPositionProvider;

        static readonly int _lazySectionSideDivision = 10;

        static PanelExtensions()
        {
            _sectonPositionProvider= new SectionPositionProvider();
        }

        public static IList<IPanelSection> CreateSections(this IPanel panel)
        {
            InitializeSectionsNumber(panel, out int nxSection, out int nySection);

            double sectionSizeX = panel.SizeX / nxSection;
            double sectionSizeY = panel.SizeY / nySection;
            double startOffsetX = -panel.SizeX / 2.0;
            double startOffsetY = -panel.SizeY / 2.0;
            double cornerX = startOffsetX;
            double cornerY = startOffsetY;
            double panelCenterZ = 0.0;

            panel.CubeSize = AdjustCubeSize(sectionSizeX, sectionSizeY, panel.SizeZ, panel.NumCells, panel.FilterMargin);

            if ((nxSection > 10) || (nySection > 10))
            {
                return CreateLazySections(panel, nxSection, nySection, sectionSizeX, sectionSizeY, cornerX, cornerY, panelCenterZ, panel.SizeZ);
            }
            else
            {
                return CreateSections(panel, _sectonPositionProvider, nxSection, nySection, sectionSizeX, sectionSizeY, cornerX, cornerY, panelCenterZ, panel.SizeZ);
            }
        }

        private static IList<IPanelSection> CreateLazySections(IPanel panel, int nxSection, int nySection, double sectionSizeX, double sectionSizeY, double cornerX, double cornerY, double panelCenterZ, double sectionSizeZ)
        {
            var nSideDiv = _lazySectionSideDivision;
            var nx = nxSection / nSideDiv;
            var ny = nySection / nSideDiv;
            var modX = nxSection% nSideDiv;
            var modY = nySection% nSideDiv;
            var cntX = (modX > 0) ? (nx + 1) : nx;
            var cntY = (modY > 0) ? (ny + 1) : ny;
            var list = new ObservableCollection<IPanelSection>();

            for (int i = 0; i < cntX; i++)
            {
                var sizeX = (i < nx) ? (sectionSizeX * nSideDiv) : (sectionSizeX * modX);
                var centerX = (i < nx) ? (cornerX + sizeX / 2.0 + sizeX * i) : (cornerX + panel.SizeX - sizeX / 2.0);

                for (int j = 0; j < cntY; j++)
                {
                    var sizeY = (j< ny) ? (sectionSizeY * nSideDiv) : (sectionSizeY * modY);
                    var centerY = (j < ny) ? (cornerY + sizeY / 2.0 + sizeY * j) : (cornerY + panel.SizeY - sizeY / 2.0);
                    var position = _sectonPositionProvider.GetSectionPosition(cntX, cntY, i, j);

                    var section = CreateLazySection(panel, position, nSideDiv, nSideDiv, sizeX, sizeY, centerY, panelCenterZ, centerX, sectionSizeZ);

                    list.Add(section);
                }
            }

            return list;
        }

        public static List<IPanelSection> CreateSections(IRemovalParameters removalParameters, ISectionPositionProvider sectonPositionProvider, int nxSection, int nySection, double sectionSizeX, double sectionSizeY, double cornerX, double cornerY, double panelCenterZ, double sectionSizeZ)
        {
            var list = new List<IPanelSection>();

            for (int i = 0; i < nxSection; i++)
            {
                var centerX = cornerX + sectionSizeX / 2.0 + sectionSizeX * i;

                for (int j = 0; j < nySection; j++)
                {
                    var centerY = cornerY + sectionSizeY / 2.0 + sectionSizeY * j;
                    var position = sectonPositionProvider.GetSectionPosition(nxSection, nySection, i, j);

                    PanelSectionViewModel section = CreateSection(removalParameters, position, sectionSizeX, sectionSizeY, centerY, panelCenterZ, centerX, sectionSizeZ);

                    list.Add(section);
                }
            }

            return list;
        }

        private static PanelSectionViewModel CreateLazySection(IRemovalParameters removalParameters, SectionPosition position, int nxSection, int nySection, double sectionSizeX, double sectionSizeY, double centerY, double panelCenterZ, double centerX, double sectionSizeZ)
        {
            var section = new LazyPanelSectionViewModel()
            {
                SectionPosition = position,
                SectionsCountX = nxSection,
                SectionsCountY = nySection,
                CenterX = centerX,
                CenterY = centerY,
                CenterZ = panelCenterZ,
                SizeX = sectionSizeX,
                SizeY = sectionSizeY,
                SizeZ = sectionSizeZ,
                RemovalParameters = removalParameters
            };
            section.Faces = section.CreateFaces(position, removalParameters);
            return section;
        }

        private static PanelSectionViewModel CreateSection(IRemovalParameters removalParameters, SectionPosition position, double sectionSizeX, double sectionSizeY, double centerY, double panelCenterZ, double centerX, double sectionSizeZ)
        {
            var section = new PanelSectionViewModel()
            {
                CenterX = centerX,
                CenterY = centerY,
                CenterZ = panelCenterZ,
                SizeX = sectionSizeX,
                SizeY = sectionSizeY,
                SizeZ = sectionSizeZ,
                RemovalParameters = removalParameters
            };

            section.Faces = section.CreateFaces(position, removalParameters);

            var vm = MVMIoc.SimpleIoc<MRVMI.IElementViewModelFactory>.GetInstance().CreateSectionVolumeViewModel();

            vm.CenterX = centerX;
            vm.CenterY = centerY;
            vm.CenterZ = panelCenterZ;
            vm.SizeX = sectionSizeX;
            vm.SizeY = sectionSizeY;
            vm.SizeZ = sectionSizeZ;
            vm.RemovalParameters = removalParameters;

            section.Volume = vm;
            return section;
        }


        private static void InitializeSectionsNumber(IPanel panel, out int nxSection, out int nySection)
        {
            nxSection = (int)Math.Ceiling(panel.SizeX / 100.0) * panel.SectionsX100mm;
            nySection = (int)Math.Ceiling(panel.SizeY / 100.0) * panel.SectionsX100mm;

            ImproveSectionsNumber(panel, ref nxSection, ref nySection);
        }

        private static void ImproveSectionsNumber(IPanel panel, ref int nxSection, ref int nySection)
        {
            var xSize = panel.SizeX / nxSection;
            var ySize = panel.SizeY / nySection;

            if (Math.Abs(xSize - ySize) > 0.01)
            {
                if (xSize > ySize)
                {
                    ImproveSectionsNumber(ySize, panel.SizeX, ref nxSection);
                }
                else
                {
                    ImproveSectionsNumber(xSize, panel.SizeY, ref nySection);
                }
            }
        }

        private static void ImproveSectionsNumber(double refSecSize, double size, ref int n)
        {
            while ((size / n) >= refSecSize) n++;

            var v1 = Math.Abs((size / n) - refSecSize);
            var v2 = Math.Abs((size / (n - 1)) - refSecSize);

            if (v1 > v2) n--;
        }

        private static SectionPosition GetSectionPosition(int nxSection, int nySection, int i, int j)
        {
            var result = SectionPosition.Center;
            bool isLeft = i == 0;
            bool isRight = i == nxSection - 1;
            bool isBottom = j == 0;
            bool isTop = j == nySection - 1;

            if (isLeft)
            {
                if (isBottom) result = SectionPosition.CornerBottomLeft;
                else if (isTop) result = SectionPosition.CornerTopLeft;
                else result = SectionPosition.SideLeft;
            }
            else if (isRight)
            {
                if (isBottom) result = SectionPosition.CornerBottomRight;
                else if (isTop) result = SectionPosition.CornerTopRight;
                else result = SectionPosition.SideRigth;
            }
            else
            {
                if (isBottom) result = SectionPosition.SideBottom;
                else if (isTop) result = SectionPosition.SideTop;
                else result = SectionPosition.Center;
            }

            return result;
        }

        private static double AdjustCubeSize(double sectionSizeX, double sectionSizeY, double sectionSizeZ, int startNumCells, double maxDiff = 0.1)
        {
            var retVal = sectionSizeX / startNumCells;

            if (sectionSizeX != sectionSizeZ)
            {
                var n1 = startNumCells;

                while (true)
                {
                    var v = sectionSizeX / n1;
                    var n2 = Math.Round(sectionSizeZ / v);
                    var d = Math.Abs(sectionSizeZ - (v * n2));

                    if (d <= maxDiff)
                    {
                        retVal = v;
                        break;
                    }

                    n1++;

                    if ((n1 - startNumCells) > 10) break;
                }                
            }

            return retVal;
        }
    }
}
