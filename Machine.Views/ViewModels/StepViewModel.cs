using Machine.ViewModels.Base;
using MachineSteps.Models.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    class StepViewModel : BaseViewModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Index { get; private set; }
        public int Channel { get; private set; }

        public StepViewModel(MachineStep step, int index = 0) : base()
        {
            Id = step.Id;
            Name = step.Name;
            Description = step.Description;
            Index = index;
            Channel = step.Channel;
        }
    }
}
