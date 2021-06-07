using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;
using static Machine.ViewModels.Interfaces.Links.ILinkMovementManager;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    struct LinksMovementsGroup : ILinksMovementsGroup
    {
        public int GroupId { get; private set; }

        public TimeSpan Duration { get; private set; }

        public DateTime Start { get; private set; }

        public bool IsCompleted { get; private set; }

        public List<IMovementItem> Items { get; private set; }
        public int NotifyId { get; set; }

        public static LinksMovementsGroup Create(int groupId, double duration, int notifyId = 0)
        {
            return new LinksMovementsGroup()
            {
                GroupId = groupId,
                Duration = TimeSpan.FromSeconds(duration),
                Start = DateTime.Now,
                NotifyId = notifyId,
                Items = new List<IMovementItem>()
            };
        }

        public void Add(ILinkViewModel link, double targetValue) => Items.Add(LinearMovementItem.Create(link, targetValue));

        public void Add(ILinkViewModel link, double targetValue, ArcComponentData data)
        {
            IArcMovementItem md = ArcMovementItem.Create(link, targetValue);

            md.Angle = data.Angle;
            md.StartAngle = data.StartAngle;
            md.CenterCoordinate = data.CenterCoordinate;
            md.Radius = data.Radius;
            md.ArcComponent = data.Component;

            Items.Add(md);
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
