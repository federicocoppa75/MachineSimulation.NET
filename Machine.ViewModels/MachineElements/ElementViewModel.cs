using Machine.Data.Base;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Messages.Probing;
using Machine.ViewModels.UI.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MVMIP = Machine.ViewModels.Interfaces.Probing;


namespace Machine.ViewModels.MachineElements
{
    [MachineStruct("Simple element", 1)]
    public class ElementViewModel : BaseViewModel, IMachineElement, IViewElementData, MVMIP.IProbableElement
    {
        #region data properties
        public int MachineElementID { get; set; }
        
        private string _name;
        public string Name 
        { 
            get => _name; 
            set => Set(ref _name, value, nameof(Name)); 
        }

        private string _modelFile;
        public string ModelFile 
        { 
            get => _modelFile; 
            set => Set(ref _modelFile, value, nameof(ModelFile)); 
        }

        private Color _color;
        public Color Color 
        { 
            get => _color; 
            set => Set(ref _color, value, nameof(Color)); 
        }

        private Matrix _transformation;
        public Matrix Transformation 
        { 
            get => _transformation; 
            set => Set(ref _transformation, value, nameof(Transformation)); 
        }
        
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

        #region IProbableElementViewModel impementation

        public void AddProbePoint(MVMIP.Point point)
        {
            var probe = GetInstance<MVMIP.IProbeFactory>()?.Create(this, point);

            Messenger.Send(new AddProbeMessage() { Probe = probe});
        }
        #endregion
    }
}
