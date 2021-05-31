using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Messages
{
    class StepCompletedMessage
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int Channel { get; set; }
    }
}
