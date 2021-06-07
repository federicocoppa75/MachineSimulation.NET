using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public enum TimeSpanFactor
    {
        Factor1,
        Factor2,
        Factor5,
        Factor10
    }

    public interface IStepsExecutionController
    {
        bool AutoStepOver { get; set; }
        bool DynamicTransition { get; set; }
        //bool MaterialRemoval { get; set; }
        bool MultiChannel { get; set; }
        TimeSpanFactor TimeSpanFactor { get; set; }
    }
}
