using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Links
{
    public class PneumaticLinkViewModel : LinkViewModel, IPneumaticLinkViewModel, ILinkViewModel
    {
        #region private properties
        private ILinkMovementManager _linkMovementManager;
        private ILinkMovementManager LinkMovementManager => _linkMovementManager ?? (_linkMovementManager = Ioc.SimpleIoc<ILinkMovementManager>.GetInstance());
        #endregion

        #region data properties
        public double OffPos { get; set; }
        
        private double _onPos;
        public double OnPos 
        { 
            get => _onPos; 
            set
            {
                if(Set(ref _onPos, value, nameof(OnPos)))
                {
                    DynOnPos = _onPos;
                }
            }
        }
        public double TOff { get; set; }
        public double TOn { get; set; }
        public bool ToolActivator { get; set; }

        private double _dynOnPos;
        public double DynOnPos 
        { 
            get => _dynOnPos; 
            set
            {
                if(Set(ref _dynOnPos, value, nameof(DynOnPos)) && _state)
                {
                    Value = _dynOnPos;
                }
            }
        }
        #endregion

        #region view properties
        private bool _state;
        public bool State
        {
            get => _state;
            set => ChangeStatus(value, -1);
        }
        public override LinkMoveType MoveType => LinkMoveType.Pneumatic;

        public event EventHandler<bool> StateChanging;
        public event EventHandler<bool> StateChanged;
        #endregion

        #region ctor
        public PneumaticLinkViewModel() : base()
        {
        }
        #endregion

        public bool ChangeStatus(bool value, int notifyId)
        {
            bool result = false;

            if (_state != value)
            {
                StateChanging?.Invoke(this, value);
                _state = value;
                ApplyStatus(_state, notifyId);
                StateChanged?.Invoke(this, _state);
                if (!_state) DynOnPos = OnPos;
                RisePropertyChanged(nameof(State));
                result = true;
            }

            return result;
        }

        private void ApplyStatus(bool state, int notifyId)
        {
            var value = state ? DynOnPos : OffPos;

            if (LinkMovementManager.Enable)
            {
                var t = state ? TOn : TOff;

                if(DynOnPos != OnPos) t *= (DynOnPos - OffPos) / (OnPos - OffPos);

                LinkMovementManager.Add(Id, Value, value, t, notifyId);
            }
            else
            {
                Value = value;
            }            
        }
    }
}
