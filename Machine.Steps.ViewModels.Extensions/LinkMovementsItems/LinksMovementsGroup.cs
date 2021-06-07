using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;
using static Machine.ViewModels.Interfaces.Links.ILinkMovementManager;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    class LinksMovementsGroup
    {
        public int GroupId { get; private set; }

        public TimeSpan Duration { get; private set; }

        public DateTime Start { get; private set; }

        public bool IsCompleted { get; private set; }

        public List<MovementItem> Items { get; private set; } = new List<MovementItem>();
        public int NotifyId { get; set; }

        public LinksMovementsGroup(int groupId, double duration, int notifyId = 0)
        {
            GroupId = groupId;
            Duration = TimeSpan.FromSeconds(duration);
            Start = DateTime.Now;
            NotifyId = notifyId;
        }

        public void Add(ILinkViewModel link, double targetValue) => Items.Add(new LinearMovementItem(link, targetValue));

        internal void Add(ILinkViewModel link, double targetValue, ArcComponentData data)
        {
            ArcMovementItem md = null;

            switch (data.Component)
            {
                case ArcComponent.X:
                    md = new XArcMovementItem(link, targetValue);
                    break;
                case ArcComponent.Y:
                    md = new YArcMovementItem(link, targetValue);
                    break;
                default:
                    break;
            }

            if (md != null)
            {
                md.Angle = data.Angle;
                md.StartAngle = data.StartAngle;
                md.CenterCoordinate = data.CenterCoordinate;
                md.Radius = data.Radius;

                Items.Add(md);
            }
            else
            {
                throw new ArgumentException("Invalid arc component!");
            }
        }

        public bool Progress(DateTime now)
        {
            bool result = false;
            var elapsed = now - Start;

            if (!IsCompleted)
            {
                if (elapsed >= Duration)
                {
                    Items.ForEach((i) => i.SetTargetValue());
                    IsCompleted = true;
                    result = true;
                }
                else
                {
                    var k = (double)elapsed.TotalMilliseconds / (double)Duration.TotalMilliseconds;

                    Items.ForEach((i) => i.SetValue(k));
                }
            }

            return result;
        }
    }
}
