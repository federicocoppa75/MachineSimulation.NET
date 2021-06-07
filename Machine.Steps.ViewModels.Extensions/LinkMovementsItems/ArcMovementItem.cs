using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions.LinkMovementsItems
{
    struct ArcMovementItem : IArcMovementItem
    {
        public double Angle { get; set; }
        public double CenterCoordinate { get; set; }
        public double Radius { get; set; }
        public double StartAngle { get; set; }
        public ILinkMovementManager.ArcComponent ArcComponent { get; set; }

        public ILinkViewModel Link { get; private set; }

        public double TargetValue { get; private set; }

        public static ArcMovementItem Create(ILinkViewModel link, double targetValue)
        {
            return new ArcMovementItem()
            {
                Link = link,
                TargetValue = targetValue
            };
        }

        public void SetValue(double k)
        {
            double a = Normalize(StartAngle + Angle * k);

            switch (ArcComponent)
            {
                case ILinkMovementManager.ArcComponent.X:
                    Link.Value = CenterCoordinate + Math.Cos(a) * Radius;
                    break;
                case ILinkMovementManager.ArcComponent.Y:
                    Link.Value = CenterCoordinate + Math.Sin(a) * Radius;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private double Normalize(double angle)
        {
            double result = angle;

            if (result > Math.PI) result -= Math.PI * 2.0;
            else if (result < (-Math.PI)) result += Math.PI * 2.0;

            return result;
        }
    }
}
