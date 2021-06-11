using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.Test.ViewModels
{
    class PanelDataViewModel : BaseViewModel
    {
        private double _sizeX = 800.0;
        public double SizeX
        {
            get => _sizeX; 
            set => Set(ref _sizeX, value, nameof(SizeX));
        }

        private double _sizeY = 600.0;
        public double SizeY
        {
            get => _sizeY;
            set => Set(ref _sizeY, value, nameof(SizeY));
        }

        private double _sizeZ = 18.0;
        public double SizeZ
        {
            get => _sizeZ;
            set => Set(ref _sizeZ, value, nameof(SizeZ));
        }
    }
}
