using Machine.Data.Base;
using Machine.ViewModels.Interfaces.Indicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Indicators
{
    public class PositionIndicatorViewModel : BaseIndicatorViewModel, IPositionIndicator
    {
        private IPositionIndicator PositionIndicator => Parent as IPositionIndicator;

        public Point Position 
        { 
            get => PositionIndicator.Position;
            set => PositionIndicator.Position = value;
        }
    }
}
