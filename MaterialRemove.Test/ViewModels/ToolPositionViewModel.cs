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
            set
            {
                var oldValue = _x;

                if(Set(ref _x, value, nameof(X)))
                {
                    DX = _x - oldValue;
                }
            }
        }

        private double _y;
        public double Y
        {
            get => _y;
            set
            {
                var oldValue = _y;

                if(Set(ref _y, value, nameof(Y)))
                {
                    DY = _y - oldValue;
                }
            }
        }

        private double _z = 150.0;
        public double Z
        {
            get => _z;
            set
            {
                var oldValue = _z;

                if(Set(ref _z, value, nameof(Z)))
                {
                    DZ = _z - oldValue;
                }
            }
        }

        private double _dx;
        public double DX
        {
            get => _dx;
            set => Set(ref _dx, value, nameof(DX));
        }

        private double _dy;
        public double DY
        {
            get => _dy;
            set => Set(ref _dy, value, nameof(DY));
        }

        private double _dz;
        public double DZ
        {
            get => _dz;
            set => Set(ref _dz, value, nameof(DZ));
        }

        public double MinValue => -1000.0;
        public double MaxValue => 1000.0;

        public void ResetD()
        {
            DX = 0.0;
            DY = 0.0;
            DZ = 0.0;
        }
    }
}
