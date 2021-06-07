using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    class LinearLinkMovementItem
    {
        public ILinkViewModel Link { get; private set; }
        public double Value { get; private set; }
        public double TargetValue { get; private set; }
        public TimeSpan Duration { get; private set; }
        public DateTime Start { get; private set; }
        public bool IsCompleted { get; set; }
        public int NotifyId { get; private set; }

        public LinearLinkMovementItem(ILinkViewModel link, double targetValue, double duration, int notifyId = 0)
        {
            Link = link;
            Value = link.Value;
            TargetValue = targetValue;
            Duration = TimeSpan.FromSeconds(duration);
            Start = DateTime.Now;
            NotifyId = notifyId;
        }

        public bool Progress(DateTime now)
        {
            bool result = false;
            var elapsed = now - Start;

            if (!IsCompleted)
            {
                if (elapsed >= Duration)
                {
                    SetValue(TargetValue);
                    IsCompleted = true;
                    result = true;
                }
                else
                {
                    var k = (double)elapsed.TotalMilliseconds / (double)Duration.TotalMilliseconds;
                    var v = (TargetValue - Value) * k + Value;

                    SetValue(v);
                }
            }

            return result;
        }

        private void SetValue(double value) => Link.Value = value;

    }
}
