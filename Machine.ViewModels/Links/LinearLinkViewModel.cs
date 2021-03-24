using Machine.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Links
{
    public class LinearLinkViewModel : LinkViewModel
    {
        #region data properties
        public double Min { get; set; }
        public double Max { get; set; }
        public double Pos { get; set; }
        #endregion

        #region view properties4
        public override LinkMoveType MoveType => LinkMoveType.Linear;
        #endregion
    }
}
