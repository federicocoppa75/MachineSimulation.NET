using Machine.Data.Base;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Handles;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Handles
{
    public class PositionHandleViewModel : BaseHandleViewModel, IPositionHandle
    {
        IMachineElement _elementToMove;
        Matrix _startTransform;

        #region IPositionHandle

        public Interfaces.Handles.Type Type { get; set; }

        public void StartMove()
        {
            _elementToMove = Parent.Parent;
            _startTransform = _elementToMove.Transformation;

            if(_startTransform == null) _startTransform = new Matrix() { M11 = 1.0, M22 = 1.0, M33 = 1.0 };
        }

        public void Move(double stepX, double stepY, double stepZ)
        {
            var m = new Matrix(_startTransform);

            m.OffsetX += stepX;
            m.OffsetY += stepY;
            m.OffsetZ += stepZ;

            _elementToMove.Transformation = m;
        }

        #endregion

        protected override string GetName() => $"Pos handle {Type}";

        #region Implementation

        #endregion
    }
}
