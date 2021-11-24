using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public class StepsExecutionControllerStub : IStepsExecutionController
    {
        public bool AutoStepOver { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DynamicTransition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool MultiChannel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpanFactor TimeSpanFactor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsStepTimeVisible { get => false; set => throw new NotImplementedException(); }
        public TimeSpan StepTime { get => TimeSpan.Zero; set => throw new NotImplementedException(); }
    }
}
