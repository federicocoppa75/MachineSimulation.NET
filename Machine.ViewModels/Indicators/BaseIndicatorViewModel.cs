using Machine.Data.Base;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Indicators;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Machine.ViewModels.Indicators
{
    public abstract class BaseIndicatorViewModel : BaseViewModel, IMachineElement, IViewElementData, IIndicatorProxy
    {
        private static ICollection<IMachineElement> _dummyChildern;

        #region IMachineElement
        public int MachineElementID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get; set; }
        public string ModelFile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color Color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Matrix Transformation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ICollection<IMachineElement> Children => _dummyChildern;
        public ILinkViewModel LinkToParent { get => null; set => throw new NotImplementedException(); }

        private IMachineElement _parent;
        public IMachineElement Parent 
        { 
            get => _parent; 
            set
            {
                if(Set(ref _parent, value, nameof(Parent)) && (_parent != null) && (_parent is INotifyPropertyChanged npc))
                {
                    npc.PropertyChanged += OnParentPropertyChanged;
                }
            }
        }
        #endregion

        #region IViewElementData        
        public bool IsVisible { get; set; } = true;
        public bool IsSelected { get => false; set => throw new NotImplementedException(); }
        public string PostEffects { get => null; set => throw new NotImplementedException(); }
        public bool IsExpanded { get => false; set => throw new NotImplementedException(); }
        #endregion

        static BaseIndicatorViewModel()
        {
            _dummyChildern = new List<IMachineElement>();
        }

        public bool TryGetIndicator<T>(out T indicator) where T : IBaseIndicator
        {
            var res = false;

            if (Parent is T i)
            {
                indicator = i;
                res = true;
            }
            else
            {
                indicator = default;
            }

            return res;
        }

        private void OnParentPropertyChanged(object sender, PropertyChangedEventArgs e) => RisePropertyChanged(e.PropertyName);

    }
}
