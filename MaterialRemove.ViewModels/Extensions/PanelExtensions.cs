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

namespace MaterialRemove.ViewModels.Extensions
{
    static class PanelExtensions
    {
        public static IList<IPanelSection> CreateSections(this IPanel panel)
        {
            var list = new List<IPanelSection>();
            InitializeSectionsNumber(panel, out int nxSection, out int nySection);

            double sectionSizeX = panel.SizeX / nxSection;
            double sectionSizeY = panel.SizeY / nySection;
            double startOffsetX = -panel.SizeX / 2.0;
            double startOffsetY = -panel.SizeY / 2.0;
            double cornerX = /*panel.CenterX +*/ startOffsetX;
            double cornerY = /*panel.CenterY +*/ startOffsetY;
            double cornerZ = /*panel.CenterZ -*/ panel.SizeZ / 2.0;

            for (int i = 0; i < nxSection; i++)
            {
                var centerX = cornerX + sectionSizeX / 2.0 + sectionSizeX * i;

                for (int j = 0; j < nySection; j++)
                {
                    var centerY = cornerY + sectionSizeY / 2.0 + sectionSizeY * j;
                    var position = GetSectionPosition(nxSection, nySection, i, j);
                    var section = new PanelSectionViewModel()
                    {
                        CenterX = centerX,
                        CenterY = centerY,
                        CenterZ = 0.0,
                        SizeX = sectionSizeX,
                        SizeY = sectionSizeY,
                        SizeZ = panel.SizeZ,
                        RemovalParameters = panel
                    };

                    section.Faces = section.CreateFaces(GetSectionPosition(nxSection, nySection, i, j), panel);
                    
                    var vm = MVMIoc.SimpleIoc<MRVMI.IElementViewModelFactory>.GetInstance().CreateSectionVolumeViewModel();

                    vm.CenterX = centerX;
                    vm.CenterY = centerY;
                    vm.CenterZ = 0.0;
                    vm.SizeX = sectionSizeX;
                    vm.SizeY = sectionSizeY;
                    vm.SizeZ = panel.SizeZ;
                    vm.RemovalParameters = panel;

                    section.Volume = vm;

                    list.Add(section);
                }
            }

            return list;
        }

        public static bool Intersect(this IPanel panel, ToolActionData toolActionData)
        {
            var panelBox = new AxisAlignedBox3d(new Vector3d(), panel.SizeX / 2.0, panel.SizeY / 2.0, panel.SizeZ / 2.0);
            var toolBox = toolActionData.GetBound();

            return panelBox.Intersects(toolBox);
        }

        public static Task<bool> IntersectAsync(this IPanel panel, ToolActionData toolActionData)
        {
            return Task.Run(async () =>
            {
                var panelBox = new AxisAlignedBox3d(new Vector3d(), panel.SizeX / 2.0, panel.SizeY / 2.0, panel.SizeZ / 2.0);
                var toolBox = await TaskHelper.ToAsync(() => toolActionData.GetBound());

                return panelBox.Intersects(toolBox);
            });
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
    }
}
