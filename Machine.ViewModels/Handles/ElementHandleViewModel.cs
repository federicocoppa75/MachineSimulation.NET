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
    public class ElementHandleViewModel : BaseViewModel, IMachineElement, IViewElementData, IElementHandle
    {
        #region IMachineElement
        public int MachineElementID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => "Element handle"; set => throw new NotImplementedException(); }
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

        #region IElementHandle
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MinZ { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double MaxZ { get; set; }
        #endregion
    }
}
