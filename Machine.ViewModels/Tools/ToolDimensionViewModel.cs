using Machine.Data.Base;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Machine.ViewModels.Tools
{
    public class ToolDimensionViewModel : BaseViewModel, IMachineElement, IViewElementData, IToolDimension
    {
        #region IMachineElement
        public int MachineElementID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get; set; }
        public string ModelFile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color Color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Matrix Transformation { get; set; }

        public ICollection<IMachineElement> Children { get; private set; } = new ObservableCollection<IMachineElement>();

        public ILinkViewModel LinkToParent { get => null; set => throw new NotImplementedException(); }
        public IMachineElement Parent { get; set; }
        #endregion

        #region IViewElementData
        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value, nameof(IsVisible));
        }

        public bool IsSelected { get => false; set => throw new NotImplementedException(); }
        public string PostEffects { get => ""; set => throw new NotImplementedException(); }
        public bool IsExpanded { get => false; set => throw new NotImplementedException(); }
        #endregion

        #region IToolDimension
        private string _propertyName;
        public string PropertyName 
        { 
            get => _propertyName; 
            set => Set(ref _propertyName, value, nameof(PropertyName));
        }

        private Point _contactPoint1;
        public Point ContactPoint1 
        { 
            get => _contactPoint1;
            set => Set(ref _contactPoint1, value, nameof(ContactPoint1));
        }

        private Point _contactPoint2;
        public Point ContactPoint2 
        { 
            get => _contactPoint2; 
            set => Set(ref _contactPoint2, value, nameof(ContactPoint2));
        }

        private Point _measurePoint1;
        public Point MeasurePoint1 
        {
            get => _measurePoint1; 
            set => Set(ref _measurePoint1, value, nameof(MeasurePoint1));
        }

        private Point _measurePoint2;
        public Point MeasurePoint2
        {
            get => _measurePoint2;
            set => Set(ref _measurePoint2, value, nameof(MeasurePoint2));
        }
        #endregion
    }
}
