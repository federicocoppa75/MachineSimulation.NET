using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Interfaces
{
    public interface IStepsContainer
    {
        string SourceName { get; set; }
        IList<StepViewModel> Steps { get; }
    }
}
