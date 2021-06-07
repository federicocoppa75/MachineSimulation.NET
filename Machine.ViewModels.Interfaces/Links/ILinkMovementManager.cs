using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Links
{
    public interface ILinkMovementManager : ILinkMovementController
    {
        public enum ArcComponent
        {
            X,
            Y
        };

        public struct ArcComponentData
        {
            public int GroupId { get; set; }
            public double StartAngle { get; set; }
            public double Angle { get; set; }
            public double CenterCoordinate { get; set; }
            public double Radius { get; set; }
            public ArcComponent Component { get; set; }
        }

        void Add(int linkId, double targetValue, double duration, int notifyId);
        void Add(int groupId, int linkId, double targetValue, double duration, int notifyId);
        void Add(int linkId, double targetValue, double duration, ArcComponentData data, int notifyId);
    }
}
