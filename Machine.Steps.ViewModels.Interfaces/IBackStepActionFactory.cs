using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Interfaces
{
    public interface IBackStepActionFactory
    {
        BaseAction Create(BaseAction action);
    }
}
