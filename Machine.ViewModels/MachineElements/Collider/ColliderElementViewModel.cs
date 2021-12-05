using Machine.Data.Base;
using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Indicators;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Machine.ViewModels.MachineElements.Collider
{
    public abstract class ColliderElementViewModel : ElementViewModel, IColliderElement, IPositionsIndicator
    {
        public abstract ColliderType Type { get; }
        public double Radius { get; set; }

        private ICollection<Point> _points;
        public virtual ICollection<Point> Points => _points ?? (_points = InstantiatePoints());
      
        public override IMachineElement Parent 
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
            IMachineElement p = this;

            while(p != null)
            {
                if(p.LinkToParent is IPneumaticLinkViewModel plink)
                {
                    CollidingDirection = GetCollidingDirection(plink);
                    plink.StateChanging += OnPneumaticLinkStateChanging;
                    plink.StateChanged += OnPneumaticLinkStateChanged;
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
        protected abstract void OnPneumaticLinkStateChanged(object sender, bool e);

        private static ICollection<Point> InstantiatePoints()
        {
            var isEditor = Ioc.SimpleIoc<IApplicationInformationProvider>.GetInstance().ApplicationType == ApplicationType.MachineEditor;

            if (isEditor)
            {
                return new ObservableCollection<Point>();
            }
            else
            {
                return new List<Point>();
            }
        }
    }
}
