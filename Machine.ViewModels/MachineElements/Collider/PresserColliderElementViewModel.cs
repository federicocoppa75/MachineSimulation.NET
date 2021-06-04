using Machine.Data.Enums;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages;
using Machine.ViewModels.Messages.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements.Collider
{
    public class PresserColliderElementViewModel : ColliderElementViewModel
    {
        IPneumaticLinkViewModel _presserLink;
        ILinkViewModel _parallelLink;
        List<ILinkViewModel> _orthogonalLinks = new List<ILinkViewModel>();
        double _intersectionDistance;
        double _intersectionLinkValue;
        bool _initialized;

        public override ColliderType Type => ColliderType.Presser;

        protected override void OnPneumaticLinkStateChanged(object sender, bool e)
        {
            if (!e) EvaluateReleasePanel(sender as IPneumaticLinkViewModel);
        }

        protected override void OnPneumaticLinkStateChanging(object sender, bool e)
        {
            if (e) EvaluatePanelCollision(sender as IPneumaticLinkViewModel);
        }

        private void EvaluateReleasePanel(IPneumaticLinkViewModel pneumaticLinkViewModel)
        {
            if(_parallelLink != null) _parallelLink.ValueChanged -= ParallelLinkValueChanged;

            foreach (var item in _orthogonalLinks)
            {
                item.ValueChanged -= OrthogonalLinkValueChanged;
            }

            _orthogonalLinks.Clear();
        }

        private void EvaluatePanelCollision(IPneumaticLinkViewModel link)
        {
            Messenger.Send(new GetPanelMessage()
            {
                SetPanel = (pvm) =>
                {
                    var th = GetInstance<IColliderHelperFactory>();
                    var ch = th.GetColliderHelper(this, pvm);

                    if (ch.Intersect(out double d))
                    {
                        _intersectionDistance = d;                        
                        ApplyPression(link, d);
                        _initialized = true;
                    }

                    _presserLink = link;
                    InitializeParallelChange();
                    InitializeOrthogonalChange(pvm);
                }
            });
        }

        private void ApplyPression(IPneumaticLinkViewModel link, double distance)
        {
            if (IsInExtensionRange(link, distance))
            {
                link.DynOnPos = distance;
            }
            else
            {
                link.DynOnPos = link.OnPos;
            }
        }

        private bool IsInExtensionRange(IPneumaticLinkViewModel link, double intersectValue)
        {
            var result = false;

            if (((link.OffPos <= intersectValue) && (intersectValue < link.OnPos)) ||
                ((link.OffPos >= intersectValue) && (intersectValue > link.OnPos)))
            {
                result = true;
            }

            return result;
        }

        private ILinearLinkViewModel GetParallelLinearLink(LinkDirection direction)
        {
            ElementViewModel p = this;

            while (p != null)
            {
                if ((p.LinkToParent is ILinearLinkViewModel link) && link.Direction == direction) return link;
                p = p.Parent;
            }

            return null;
        }

        private void GetOrthogonalLinearLink(LinkDirection direction)
        {
            Messenger.Send(new GetLinkMessage()
            {
                SetLink = (link) =>
                {
                    if((link.MoveType == LinkMoveType.Linear) &&
                        (link.Type == LinkType.Linear) &&
                        (link.Direction != direction))
                    {
                        _orthogonalLinks.Add(link);
                    }
                }
            });
        }

        private void InitializeParallelChange()
        {
            _parallelLink = GetParallelLinearLink(_presserLink.Direction);
            _parallelLink.ValueChanged += ParallelLinkValueChanged;
            _intersectionLinkValue = _parallelLink.Value;
        }

        private void ParallelLinkValueChanged(object sender, double e)
        {
            if(!_initialized)
            {
                Messenger.Send(new GetPanelMessage()
                {
                    SetPanel = (pvm) =>
                    {
                        var th = GetInstance<IColliderHelperFactory>();
                        var ch = th.GetColliderHelper(this, pvm);

                        if (ch.Intersect(out double d))
                        {
                            _intersectionDistance = d + _presserLink.DynOnPos;
                            ApplyPression(_presserLink, _intersectionDistance);
                            _intersectionLinkValue = _parallelLink.Value;
                            _initialized = true;
                        }
                        else
                        {
                            _presserLink.DynOnPos = _presserLink.OnPos;
                        }
                    }
                });
            }
            else
            {
                var d = _parallelLink.Value - _intersectionLinkValue;
                ApplyPression(_presserLink, _intersectionDistance - d);
            }
        }

        private void InitializeOrthogonalChange(PanelViewModel pvm)
        {
            GetOrthogonalLinearLink(_presserLink.Direction);

            foreach (var item in _orthogonalLinks) item.ValueChanged += OrthogonalLinkValueChanged;
        }

        private void OrthogonalLinkValueChanged(object sender, double e) => _initialized = false;
    }
}
