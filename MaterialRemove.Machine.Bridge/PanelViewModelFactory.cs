using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;
using MVMIoc = Machine.ViewModels.Ioc;
using MVMI = Machine.ViewModels.Interfaces;
using MaterialRemove.Interfaces;
using MaterialRemove.Interfaces.Enums;

namespace MaterialRemove.Machine.Bridge
{
    public class PanelViewModelFactory : IPanelElementFactory
    {
        public IPanelElement Create(double centerX, double centerY, double centerZ, double sizeX, double sizeY, double sizeZ)
        {
            var materialRemoveData = MVMIoc.SimpleIoc<IMaterialRemoveData>.GetInstance();

            var panel = new PanelViewModel()
            {
                CenterX = centerX,
                CenterY = centerY,
                CenterZ = centerZ,
                SizeX = sizeX,
                SizeY = sizeY,
                SizeZ = sizeZ,
                NumCells = 16,
                SectionsX100mm = 3,
                FilterMargin = 0.1,
                PanelFragment = materialRemoveData.PanelFragment,
                SectionDivision = Convert(materialRemoveData.SectionDivision),
            };

            panel.Initialize();

            return panel;
        }

        private static int Convert(SectionDivision value)
        {
            switch (value)
            {
                case SectionDivision.By_5:
                    return 5;
                case SectionDivision.By_8:
                    return 8;
                case SectionDivision.By_10:
                    return 10;
                case SectionDivision.By_12:
                    return 12;
                case SectionDivision.By_15:
                    return 15;
                case SectionDivision.By_20:
                    return 20;
                default:
                    throw new ArgumentOutOfRangeException($"Value {value} not managed!");
            }
        }
    }
}
