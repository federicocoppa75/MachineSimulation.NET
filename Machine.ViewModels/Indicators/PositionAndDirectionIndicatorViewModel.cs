using Machine.Data.Base;
using Machine.ViewModels.Interfaces.Indicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Indicators
{
    public class PositionAndDirectionIndicatorViewModel : PositionIndicatorViewModel, IPositionAndDirectionIndicator
    {
        private IPositionAndDirectionIndicator PositionAndDirectionIndicator => Parent as IPositionAndDirectionIndicator;

        public Vector Direction 
        { 
            get => PositionAndDirectionIndicator.Direction; 
            set => PositionAndDirectionIndicator.Direction = value; 
        }
    }
}
