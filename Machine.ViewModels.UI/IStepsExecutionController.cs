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

    public enum SampleTimeOption
    {
        Sample_20ms,
        Sample_30ms,
        Sample_40ms,
        Sample_50ms
    }

    public interface IStepsExecutionController
    {
        bool AutoStepOver { get; set; }
        bool DynamicTransition { get; set; }
        bool MultiChannel { get; set; }
        TimeSpanFactor TimeSpanFactor { get; set; }
        bool IsStepTimeVisible { get; set; }
        TimeSpan StepTime { get; set; }
        SampleTimeOption MinimumSampleTime { get; set; }
    }
}
