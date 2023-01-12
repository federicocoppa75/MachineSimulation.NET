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
            SizeX = this.SizeX + 10,
            SizeY = this.SizeY + 10,
            SizeZ = this.SizeZ + 10,
        };

        public IList<IPanelSection> GetSubSections()
        {
            var sections = PanelExtensions.CreateSections(RemovalParameters,
                                                          this,
                                                          SectionsCountX,
                                                          SectionsCountY,
                                                          SizeX / SectionsCountX,
                                                          SizeY / SectionsCountY,
                                                          CenterX - SizeX / 2.0,
                                                          CenterY - SizeY / 2.0,
                                                          CenterZ,
                                                          SizeZ);

            return sections;
        }

        public SectionPosition GetSectionPosition(int nxSection, int nySection, int i, int j)
        {
            var result = SectionPosition.Center;

            switch (SectionPosition)
            {
                case SectionPosition.Center:
                    result = SectionPosition.Center;
                    break;
                case SectionPosition.SideTop:
                    result = (j == nySection - 1) ? SectionPosition.SideTop : SectionPosition.Center;
                    break;
                case SectionPosition.SideRigth:
                    result = (i == nxSection - 1) ? SectionPosition.SideRigth : SectionPosition.Center;
                    break;
                case SectionPosition.SideBottom:
                    result = (j == 0) ? SectionPosition.SideBottom : SectionPosition.Center;
                    break;
                case SectionPosition.SideLeft:
                    result = (i == 0) ? SectionPosition.SideLeft : SectionPosition.Center;
                    break;
                case SectionPosition.CornerTopRight:
                    result = GetSectionPosition(() => (j == nySection - 1), () => (i == nxSection - 1), SectionPosition.SideTop, SectionPosition.SideRigth);
                    break;
                case SectionPosition.CornerTopLeft:
                    result = GetSectionPosition(() => (j == nySection - 1), () => (i == 0), SectionPosition.SideTop, SectionPosition.SideLeft);
                    break;
                case SectionPosition.CornerBottomLeft:
                    result = GetSectionPosition(() => (j == 0), () => (i == 0), SectionPosition.SideBottom, SectionPosition.SideLeft);
                    break;
                case SectionPosition.CornerBottomRight:
                    result = GetSectionPosition(() => (j == 0), () => (i == nxSection - 1), SectionPosition.SideBottom, SectionPosition.SideRigth);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        static SectionPosition GetSectionPosition(Func<bool> funcIsOnSide1, Func<bool> funcIsOnSide2, SectionPosition side1, SectionPosition side2)
        {
            var isOnSide1 = funcIsOnSide1();
            var isOnSide2 = funcIsOnSide2();

            if(isOnSide1 && isOnSide2)
            {
                return GetCorner(side1, side2);
            }
            else if(isOnSide1 && !isOnSide2)
            {
                return side1;
            }
            else if(!isOnSide1 && isOnSide2)
            {
                return side2;
            }
            else
            {
                return SectionPosition.Center;
            }
        }

        static SectionPosition GetCorner(SectionPosition side1, SectionPosition side2)
        {
            if (IsCornerBomomLeft(side1, side2)) return SectionPosition.CornerBottomLeft;
            else if (IsCornerBomomRight(side1, side2)) return SectionPosition.CornerBottomRight;
            else if (IsCornerTopLeft(side1, side2)) return SectionPosition.CornerTopLeft;
            else if (IsCornerTopRight(side1, side2)) return SectionPosition.CornerTopRight;
            else throw new ArgumentException($"No corner condition with {side1} and {side2}");
        }

        static bool IsCornerBomomLeft(SectionPosition side1, SectionPosition side2) => IsCorner(side1, side2, SectionPosition.SideBottom, SectionPosition.SideLeft);
        static bool IsCornerBomomRight(SectionPosition side1, SectionPosition side2) => IsCorner(side1, side2, SectionPosition.SideBottom, SectionPosition.SideRigth);
        static bool IsCornerTopLeft(SectionPosition side1, SectionPosition side2) => IsCorner(side1, side2, SectionPosition.SideTop, SectionPosition.SideLeft);
        static bool IsCornerTopRight(SectionPosition side1, SectionPosition side2) => IsCorner(side1, side2, SectionPosition.SideTop, SectionPosition.SideRigth);


        static bool IsCorner(SectionPosition side1, SectionPosition side2, SectionPosition request1, SectionPosition request2)
        {
            return ((side1 == request1) && (side2 == request2)) ||
                    ((side1 == request2) && (side2 == request1));
        }
    }
}
