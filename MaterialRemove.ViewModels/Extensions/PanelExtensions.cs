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

        public struct SectionSize
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

        public struct Position
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        public struct SectionDivision
        {
            public int X;
            public int Y;
        }

        static ISectionPositionProvider _sectonPositionProvider;

        static PanelExtensions()
        {
            _sectonPositionProvider= new SectionPositionProvider();
        }

        public static IList<IPanelSection> CreateSections(this IPanel panel)
        {
            InitializeSectionsNumber(panel, out int nxSection, out int nySection);

            var nSideDiv = panel.SectionDivision;
            double panelCenterZ = 0.0;

            var sectionDivision = new SectionDivision()
            {
                X = nxSection,
                Y = nySection
            };
            var sectionSize = new SectionSize()
            {
                X = panel.SizeX / nxSection,
                Y = panel.SizeY / nySection,
                Z = panel.SizeZ
            };
            var corner = new Position()
            {
                X = -panel.SizeX / 2.0,
                Y = -panel.SizeY / 2.0
            };

            panel.CubeSize = AdjustCubeSize(sectionSize.X, sectionSize.Y, panel.SizeZ, panel.NumCells, panel.FilterMargin);

            if ((panel.PanelFragment == MaterialRemove.Interfaces.Enums.PanelFragment.Lazy) && 
                ((nxSection > nSideDiv) || (nySection > nSideDiv)))
            {
                return CreateLazySections(panel, sectionDivision, sectionSize, corner, panelCenterZ);
            }
            else
            {
                return CreateSections(panel, _sectonPositionProvider, sectionDivision, sectionSize, corner, panelCenterZ);
            }
        }

        private static IList<IPanelSection> CreateLazySections(IPanel panel, SectionDivision sectionDivision, SectionSize size, Position corner, double panelCenterZ)
        {
            var nSideDiv = panel.SectionDivision;
            var nx = sectionDivision.X / nSideDiv;
            var ny = sectionDivision.Y / nSideDiv;
            var modX = sectionDivision.X % nSideDiv;
            var modY = sectionDivision.Y % nSideDiv;
            var cntX = (modX > 0) ? (nx + 1) : nx;
            var cntY = (modY > 0) ? (ny + 1) : ny;
            var list = new ObservableCollection<IPanelSection>();
            var sideDiv = new SectionDivision() { X = nSideDiv, Y = nSideDiv };

            for (int i = 0; i < cntX; i++)
            {
                var sizeX = (i < nx) ? (size.X * nSideDiv) : (size.X * modX);
                var centerX = (i < nx) ? (corner.X + sizeX / 2.0 + sizeX * i) : (corner.X + panel.SizeX - sizeX / 2.0);

                for (int j = 0; j < cntY; j++)
                {
                    var sizeY = (j< ny) ? (size.Y * nSideDiv) : (size.Y * modY);
                    var centerY = (j < ny) ? (corner.Y + sizeY / 2.0 + sizeY * j) : (corner.Y + panel.SizeY - sizeY / 2.0);
                    var position = GetLazySectionPosition(cntX, cntY, i, j);
                    var secSize = new SectionSize() { X = sizeX, Y= sizeY, Z = size.Z };
                    var center = new Position() { X = centerX, Y = centerY };

                    var section = CreateLazySection(panel, position, sideDiv, secSize, center, panelCenterZ);

                    list.Add(section);
                }
            }

            return list;
        }

        static SectionPosition GetLazySectionPosition(int nxSection, int nySection, int i, int j)
        {
            if (nxSection == 1)
            {
                return LazySectionExtension.GetSectionPositionY(nySection, j);
            }
            else if (nySection == 1)
            {
                return LazySectionExtension.GetSectionPositionX(nxSection, i);
            }
            else
            {
                return _sectonPositionProvider.GetSectionPosition(nxSection, nySection, i, j);
            }
        }


        public static List<IPanelSection> CreateSections(IRemovalParameters removalParameters, ISectionPositionProvider sectonPositionProvider, SectionDivision sectionDivision, SectionSize size, Position corner, double panelCenterZ)
        {
            var list = new List<IPanelSection>();

            for (int i = 0; i < sectionDivision.X; i++)
            {
                var centerX = corner.X + size.X / 2.0 + size.X * i;

                for (int j = 0; j < sectionDivision.Y; j++)
                {
                    var centerY = corner.Y + size.Y / 2.0 + size.Y * j;
                    var position = sectonPositionProvider.GetSectionPosition(sectionDivision.X, sectionDivision.Y, i, j);
                    var center = new Position() { X = centerX, Y = centerY };

                    PanelSectionViewModel section = CreateSection(removalParameters, position, size, center, panelCenterZ);

                    list.Add(section);
                }
            }

            return list;
        }

        private static PanelSectionViewModel CreateLazySection(IRemovalParameters removalParameters, SectionPosition position, SectionDivision sectionDivision, SectionSize size, Position center, double panelCenterZ)
        {
            var section = new LazyPanelSectionViewModel()
            {
                SectionPosition = position,
                SectionsCountX = sectionDivision.X,
                SectionsCountY = sectionDivision.Y,
                CenterX = center.X,
                CenterY = center.Y,
                CenterZ = panelCenterZ,
                SizeX = size.X,
                SizeY = size.Y,
                SizeZ = size.Z,
                RemovalParameters = removalParameters
            };
            section.Faces = section.CreateFaces(position, removalParameters);
            return section;
        }

        private static PanelSectionViewModel CreateSection(IRemovalParameters removalParameters, SectionPosition position, SectionSize size, Position center, double panelCenterZ)
        {
            var section = new PanelSectionViewModel()
            {
                CenterX = center.X,
                CenterY = center.Y,
                CenterZ = panelCenterZ,
                SizeX = size.X,
                SizeY = size.Y,
                SizeZ = size.Z,
                RemovalParameters = removalParameters
            };

            section.Faces = section.CreateFaces(position, removalParameters);

            var vm = MVMIoc.SimpleIoc<MRVMI.IElementViewModelFactory>.GetInstance().CreateSectionVolumeViewModel();

            vm.CenterX = center.X;
            vm.CenterY = center.Y;
            vm.CenterZ = panelCenterZ;
            vm.SizeX = size.X;
            vm.SizeY = size.Y;
            vm.SizeZ = size.Z;
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
