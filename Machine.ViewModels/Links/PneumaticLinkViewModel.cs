using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Links
{
    public class PneumaticLinkViewModel : LinkViewModel
    {
        #region data properties
        public double OffPos { get; set; }
        public double OnPos { get; set; }
        public double TOff { get; set; }
        public double TOn { get; set; }
        public bool ToolActivator { get; set; }
        #endregion

        #region view properties
        private bool _state;
        public bool State 
        {
            get => _state; 
            set
            {
                if(Set(ref _state, value, nameof(State)))
                {
                    Value = _state ? OnPos : OffPos;
                }
            }
        }
        public override LinkMoveType MoveType => LinkMoveType.Pneumatic;
        #endregion

        #region ctor
        public PneumaticLinkViewModel() : base()
        {
        }
        #endregion
    }
}
