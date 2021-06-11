using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.Test.ViewModels
{
    class ToolPositionViewModel : BaseViewModel
    {
        private double _x;
        public double X
        {
            get => _x; 
            set => Set(ref _x, value, nameof(X)); 
        }

        private double _y;
        public double Y
        {
            get => _y;
            set => Set(ref _y, value, nameof(Y));
        }

        private double _z = 150.0;
        public double Z
        {
            get => _z;
            set => Set(ref _z, value, nameof(Z));
        }

        public double MinValue => -1000.0;
        public double MaxValue => 1000.0;
    }
}
