using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Factories
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
                SizeZ = sizeZ
            };

            return panel;
        }
    }
}
