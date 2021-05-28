using Machine.ViewModels.Base;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels
{
    public class ActionViewModel : BaseViewModel
    {
        static int _idSeed = 1;

        public int Id { get; private set; }

        public BaseAction Action { get; private set; }

        private bool _durationIsValid;
        private double _duration;

        public double Duration
        {
            get
            {
                //if (!_durationIsValid) InitDuration();
                return _duration;
            }
        }

        public bool IsCompleted { get; set; }
    }
}
