using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.UI.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Links
{
    [Link("Pneumatic")]
    public class PneumaticLinkViewModel : LinkViewModel, IPneumaticLinkViewModel, ILinkViewModel
    {
        #region private properties
        private ILinkMovementController _linkMovementController;
        private ILinkMovementController LinkMovementController
        {
            get
            {
                if(_linkMovementController == null)
                {
                    _linkMovementController = Ioc.SimpleIoc<ILinkMovementManager>.TryGetInstance(out var manager) ? manager : Ioc.SimpleIoc<ILinkMovementController>.GetInstance();
                }

                return _linkMovementController;
            }
        }
            
        #endregion

        #region data properties
        private double _offPos;
        public double OffPos 
        { 
            get => _offPos; 
            set => Set(ref _offPos, value, nameof(OffPos)); 
        }
        
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

        private double _tOff;
        public double TOff 
        { 
            get => _tOff; 
            set => Set(ref _tOff, value, nameof(TOff)); 
        }

        private double _tOn;
        public double TOn 
        { 
            get => _tOn; 
            set => Set(ref _tOn, value, nameof(TOn)); 
        }

        private bool _toolActivator;
        public bool ToolActivator 
        { 
            get => _toolActivator; 
            set => Set(ref _toolActivator, value, nameof(ToolActivator)); 
        }

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

        private bool _firstSubscribe = true;
        private event EventHandler<bool> _stateChangeCompleted;
        public event EventHandler<bool> StateChangeCompleted
        {
            add
            {
                if(_firstSubscribe)
                {
                    ValueChanged += OnValueChanged;
                    _firstSubscribe = false;
                }

                _stateChangeCompleted += value;
            }
            remove { _stateChangeCompleted -= value; }
        }
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

            if (LinkMovementController.Enable && (LinkMovementController is ILinkMovementManager manager))
            {
                var t = state ? TOn : TOff;

                if(DynOnPos != OnPos) t *= (DynOnPos - OffPos) / (OnPos - OffPos);

                manager.Add(Id, value, t, notifyId);
            }
            else
            {
                Value = value;
            }            
        }

        private void OnValueChanged(object sender, double e)
        {
            var pos = State ? DynOnPos : OffPos;

            if(e == pos)
            {
                _stateChangeCompleted?.Invoke(this, State);
            }
        }
    }
}
