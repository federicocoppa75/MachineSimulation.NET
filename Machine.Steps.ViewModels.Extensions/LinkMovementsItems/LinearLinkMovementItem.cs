using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    class LinearLinkMovementItem
    {
        public int LinkId { get; private set; }

        public double Value { get; private set; }

        public double TargetValue { get; private set; }

        public double ActualValue { get; set; }

        public TimeSpan Duration { get; private set; }

        public DateTime Start { get; private set; }

        public bool IsCompleted { get; set; }
        public int NotifyId { get; private set; }

        public LinearLinkMovementItem(int linkId, double value, double targetValue, double duration, int notifyId = 0)
        {
            LinkId = linkId;
            Value = value;
            TargetValue = targetValue;
            Duration = TimeSpan.FromSeconds(duration * 2.0);
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

        private void SetValue(double value) => ActualValue = value;

    }
}
