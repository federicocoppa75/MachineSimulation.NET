using System;
using Machine.Data.Base;
using Machine.Data.Interfaces.Tools;

namespace Machine.ViewModels.Messages.Tooling
{
    public class AngularTransmissionLoadMessage
    {
        public int ToolHolder { get; set; }
        public IAngularTransmission AngularTransmission { get; set; }
        public Action<Action<Point, Vector, ITool>> AppendSubSpindle { get; set; }
    }
}
