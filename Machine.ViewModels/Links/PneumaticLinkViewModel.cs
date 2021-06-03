using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Links
{
    public class PneumaticLinkViewModel : LinkViewModel, IPneumaticLinkViewModel, ILinkViewModel
    {
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
        public double DynOnPos { get; set; }
        #endregion

        #region view properties
        private bool _state;
        public bool State
        {
            get => _state;
            set
            {
                if (Set(ref _state, value, nameof(State)))
                {
                    StateChanging?.Invoke(this, value);
                    Value = _state ? DynOnPos : OffPos;
                    StateChanged?.Invoke(this, _state);
                    if (!_state) DynOnPos = OnPos;
                }
            }
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
    }
}
