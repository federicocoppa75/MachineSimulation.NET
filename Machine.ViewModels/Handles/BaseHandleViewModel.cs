using Machine.Data.Base;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Handles
{
    public abstract class BaseHandleViewModel : BaseViewModel, IMachineElement, IViewElementData
    {
        #region IMachineElement
        public int MachineElementID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => $"Handle {GetName()}"; set => throw new NotImplementedException(); }
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

        protected abstract string GetName();
    }
}
