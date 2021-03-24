using Machine.Data.Links;
using Machine.Data.MachineElements;
using Machine.ViewModels.Base;
using Machine.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Machine.ViewModels.MachineElements
{
    public class ElementViewModel : BaseViewModel
    {
        #region data properties
        public int MachineElementID { get; set; }
        public string Name { get; set; }
        public string ModelFile { get; set; }
        public Color Color { get; set; }
        public Matrix Transformation { get; set; }
        public ICollection<ElementViewModel> Children { get; protected set; } = new ObservableCollection<ElementViewModel>();

        private LinkViewModel _linkToParent;
        public LinkViewModel LinkToParent 
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

        public ElementViewModel Parent { get; set; }
         #endregion

        #region commands
        private ICommand _changeChainVisibilityState;
        public ICommand ChangeChainVisibilityState { get { return _changeChainVisibilityState ?? (_changeChainVisibilityState = new RelayCommand(() => ChangeChainVisibilityStateImpl())); } }
        #endregion


        //public static explicit operator ElementViewModel(MachineElement machineElement)
        //{
        //    var evm = new ElementViewModel()
        //    {
        //        MachineElementID = machineElement.MachineElementID,
        //        Name = machineElement.Name,
        //        ModelFile = machineElement.ModelFile,
        //        Color = machineElement.Color,
        //        Transformation = machineElement.Transformation,
        //        LinkToParent = ConvertLink(machineElement.LinkToParent, machineElement.Name)
        //    };

        //    foreach (var item in machineElement.Children) evm.Children.Add((ElementViewModel)item);

        //    foreach (var item in evm.Children) item.Parent = evm;

        //    return evm;
        //}

        private static LinkViewModel ConvertLink(Link linkToParent, string name)
        {
            LinkViewModel vm = null;

            if(linkToParent is LinearLink linearLink)
            {
                vm = new LinearLinkViewModel()
                {
                    Id = linkToParent.Id,
                    LinkID = linearLink.LinkID,
                    Direction = linearLink.Direction,
                    Type = linearLink.Type,
                    Max = linearLink.Max,
                    Min = linearLink.Min,
                    Pos = linearLink.Pos,
                    Value = linearLink.Pos,
                    Description = name
                };
            }
            else if(linkToParent is PneumaticLink pneumaticLink)
            {
                vm = new PneumaticLinkViewModel()
                {
                    Id = pneumaticLink.Id,
                    LinkID = pneumaticLink.LinkID,
                    Direction = pneumaticLink.Direction,
                    Type = pneumaticLink.Type,
                    OffPos = pneumaticLink.OffPos,
                    OnPos = pneumaticLink.OnPos,
                    TOff = pneumaticLink.TOff,
                    TOn = pneumaticLink.TOn,
                    ToolActivator = pneumaticLink.ToolActivator,
                    Description = name
                };
            }

            return vm;
        }

        #region implementation
        private static void RequestTreeviewVisibility(ElementViewModel vm)
        {
            if ((vm != null) && !vm.IsExpanded)
            {
                RequestTreeviewVisibility(vm.Parent);
                vm.IsExpanded = true;
            }
        }

        private void ChangeChainVisibilityStateImpl() => ChangeVisibleProperty(this, !IsVisible);

        private void ChangeVisibleProperty(ElementViewModel vm, bool value)
        {
            vm.IsVisible = value;
            ChangeChildrenVisibleProperty(vm, value);
        }

        private void ChangeChildrenVisibleProperty(ElementViewModel me, bool value)
        {
            foreach (var item in me.Children)
            {
                ChangeVisibleProperty(item, value);
            }
        }

        #endregion
    }
}
