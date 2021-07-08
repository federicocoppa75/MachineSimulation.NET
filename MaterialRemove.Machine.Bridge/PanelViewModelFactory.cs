using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;
using MVMIoc = Machine.ViewModels.Ioc;
using MVMI = Machine.ViewModels.Interfaces;

namespace MaterialRemove.Machine.Bridge
{
    public class PanelViewModelFactory : IPanelElementFactory
    {
        public IPanelElement Create(double centerX, double centerY, double centerZ, double sizeX, double sizeY, double sizeZ)
        {
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
                FilterMargin = 0.1
            };

            panel.Initialize();

            return panel;
        }
    }
}
