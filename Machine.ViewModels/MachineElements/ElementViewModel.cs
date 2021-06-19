using Machine.Data.Base;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Machine.ViewModels.MachineElements
{
    public class ElementViewModel : BaseViewModel, IMachineElement, IViewElementData
    {
        #region data properties
        public int MachineElementID { get; set; }
        public string Name { get; set; }
        public string ModelFile { get; set; }
        public Color Color { get; set; }
        public Matrix Transformation { get; set; }
        public ICollection<IMachineElement> Children { get; protected set; } = new ObservableCollection<IMachineElement>();

        private ILinkViewModel _linkToParent;
        public ILinkViewModel LinkToParent 
        { 
            get => _linkToParent;
            set => Set(ref _linkToParent, value, nameof(LinkToParent));
        }
        #endregion

        #region view properties
        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible; 
            set => Set(ref _isVisible, value, nameof(IsVisible));
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if(Set(ref _isSelected, value, nameof(IsSelected)))
                {
                    var kernel = Ioc.SimpleIoc<IKernelViewModel>.GetInstance();
                    if (kernel != null) kernel.Selected = _isSelected ? this : null;
                    PostEffects = _isSelected ? $"highlight[color:#FFFF00]" : null;
                    if (_isSelected) RequestTreeviewVisibility(Parent);
                }
            }
        }

        private string _postEffects;
        public string PostEffects
        {
            get => _postEffects;
            set => Set(ref _postEffects, value, nameof(PostEffects));
        }

        private bool _isExpanded;
        public bool IsExpanded 
        {
            get => _isExpanded; 
            set => Set(ref _isExpanded, value, nameof(IsExpanded)); 
        }

        public virtual IMachineElement Parent { get; set; }
         #endregion

        #region commands
        private ICommand _changeChainVisibilityState;
        public ICommand ChangeChainVisibilityState { get { return _changeChainVisibilityState ?? (_changeChainVisibilityState = new RelayCommand(() => ChangeChainVisibilityStateImpl())); } }
        #endregion

        #region implementation
        private static void RequestTreeviewVisibility(IMachineElement me)
        {
            if ((me != null) && (me is IViewElementData ved) && !ved.IsExpanded)
            {
                RequestTreeviewVisibility(me.Parent);
                ved.IsExpanded = true;
            }
        }

        private void ChangeChainVisibilityStateImpl() => ChangeVisibleProperty(this, !IsVisible);

        private void ChangeVisibleProperty(IMachineElement me, bool value)
        {
            if(me is IViewElementData ved) ved.IsVisible = value;
            ChangeChildrenVisibleProperty(me, value);
        }

        private void ChangeChildrenVisibleProperty(IMachineElement me, bool value)
        {
            foreach (var item in me.Children)
            {
                ChangeVisibleProperty(item, value);
            }
        }

        #endregion
    }
}
