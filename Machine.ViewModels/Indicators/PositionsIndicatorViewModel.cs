using Machine.Data.Base;
using Machine.ViewModels.Interfaces.Indicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Indicators
{
    public class PositionsIndicatorViewModel : BaseIndicatorViewModel, IPositionsIndicator
    {
        private IPositionsIndicator PositionsIndicator => Parent as IPositionsIndicator;

        public ICollection<Point> Points => PositionsIndicator.Points;
    }
}
