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
    public class PositionHandleViewModel : BaseViewModel, IMachineElement, IViewElementData, IPositionHandle
    {
        IMachineElement _elementToMove;
        Matrix _startTransform;

        #region IMachineElement
        public int MachineElementID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => $"Handle {Type}"; set => throw new NotImplementedException(); }
        public string ModelFile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color Color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Matrix Transformation { get; set; }

        public ICollection<IMachineElement> Children { get; private set; } = new List<IMachineElement>();

        public ILinkViewModel LinkToParent { get => null; set => throw new NotImplementedException(); }
        public IMachineElement Parent { get; set; }
        #endregion

        #region IViewElementData
        public bool IsVisible { get; set; }
        public bool IsSelected { get => false; set => throw new NotImplementedException(); }
        public string PostEffects { get => ""; set => throw new NotImplementedException(); }
        public bool IsExpanded { get; set; }
        #endregion

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

        #region Implementation

        #endregion
    }
}
