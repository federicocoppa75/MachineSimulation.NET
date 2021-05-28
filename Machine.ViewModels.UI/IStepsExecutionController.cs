using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface IStepsExecutionController
    {
        bool AutoStepOver { get; set; }
        bool DynamicTransition { get; set; }
        //bool MaterialRemoval { get; set; }
        bool MultiChannel { get; set; }
    }
}
