using Machine.Data.Base;
using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;

namespace Machine.ViewModels.MachineElements.Collider
{
    public abstract class ColliderElementViewModel : ElementViewModel
    {
        public abstract ColliderType Type { get; }
        public double Radius { get; set; }
        public virtual ICollection<Point> Points { get; set; } = new List<Point>();
        public override ElementViewModel Parent 
        {
            get => base.Parent;
            set
            {
                base.Parent = value;
                if (Parent != null) AttachActivator();
            }
        }
        public Vector CollidingDirection { get; private set; }

        private void AttachActivator()
        {
            ElementViewModel p = this;

            while(p != null)
            {
                if(p.LinkToParent is IPneumaticLinkViewModel plink)
                {
                    CollidingDirection = GetCollidingDirection(plink);
                    plink.StateChanging += OnPneumaticLinkStateChanging;
                    break;
                }

                p = p.Parent;
            }
        }

        private Vector GetCollidingDirection(IPneumaticLinkViewModel plink)
        {
            var v = plink.OnPos - plink.OffPos;

            switch (plink.Direction)
            {
                case LinkDirection.X:
                    return (v > 0) ? new Vector() { X = 1.0, Y = 0.0, Z = 0.0 } : new Vector() { X = -1.0, Y = 0.0, Z = 0.0 };
                case LinkDirection.Y:
                    return (v > 0) ? new Vector() { X = 0.0, Y = 1.0, Z = 0.0 } : new Vector() { X = 0.0, Y = -1.0, Z = 0.0 };
                case LinkDirection.Z:
                    return (v > 0) ? new Vector() { X = 0.0, Y = 0.0, Z = 1.0 } : new Vector() { X = 0.0, Y = 0.0, Z = -1.0 };
                default:
                    throw new ArgumentException();
            }
        }

        protected abstract void OnPneumaticLinkStateChanging(object sender, bool e);
    }
}
