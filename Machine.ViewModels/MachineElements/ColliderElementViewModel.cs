using Machine.Data.Base;
using Machine.Data.Enums;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class ColliderElementViewModel : ElementViewModel
    {
        public ColliderType Type { get; set; }
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

        private void OnPneumaticLinkStateChanging(object sender, bool e)
        {
            if(e && Type == ColliderType.Gripper) EvaluatePanelGripperCollision(sender as IPneumaticLinkViewModel);
        }

        private void EvaluatePanelGripperCollision(IPneumaticLinkViewModel link)
        {
            Messenger.Send(new GetPanelMessage()
            {
                SetPanel = (pvm) =>
                {
                    var th = GetInstance<IColliderHelperFactory>();
                    var ch = th.GetColliderHelper(this, pvm);

                    if(ch.Intersect(out double d))
                    {
                        link.DynOnPos = d;
                    }
                }
            });
        }
    }
}
