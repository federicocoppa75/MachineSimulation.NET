using Machine.ViewModels.Base;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.Test.ViewModels
{
    class ToolDataViewModel : BaseViewModel
    {
        private double _radius = 10.0;
        public double Radius
        {
            get => _radius; 
            set => Set(ref _radius, value, nameof(Radius));
        }

        private double _length = 100.0;
        public double Length
        {
            get => _length;
            set => Set(ref _length, value, nameof(Length));
        }

        private int _repetition = 1;
        public int Repetition
        {
            get => _repetition; 
            set => Set(ref _repetition, value, nameof(Repetition));
        }

        private double _stepX = 32.0;
        public double StepX
        {
            get => _stepX; 
            set => Set(ref _stepX, value, nameof(StepX));
        }

        private double _stepY = 32.0;
        public double StepY
        {
            get => _stepY;
            set => Set(ref _stepY, value, nameof(StepY));
        }

        private Orientation _direction = Orientation.ZNeg;

        public Orientation Direction
        {
            get => _direction;
            set => Set(ref _direction, value, nameof(Direction));
        }

    }
}
