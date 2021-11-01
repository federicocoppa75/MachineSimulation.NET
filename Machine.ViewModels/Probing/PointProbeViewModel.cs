using Machine.Data.Base;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Probing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.Probing
{
    public class PointProbeViewModel : BaseViewModel, IMachineElement, IViewElementData, IProbe, IProbePoint, IProbePointChangable
    {
        #region IProbe
        public int ProbeId { get; set; }

        private double _x;
        public double X 
        { 
            get => _x; 
            set => Set(ref _x, value, nameof(X)); 
        }

        private double _y;
        public double Y
        {
            get => _y;
            set => Set(ref _y, value, nameof(Y));
        }

        private double _z;
        public double Z
        {
            get => _z;
            set => Set(ref _z, value, nameof(Z));
        }
        #endregion

        #region IProbePoint
        public double RelativeX { get; set; }
        public double RelativeY { get; set; }
        public double RelativeZ { get; set; }
        #endregion

        #region IProbePointChangable
        private IProbePointChangableTransformer _transformer;
        public IProbePointChangableTransformer Transformer 
        { 
            get => _transformer;
            set
            {
                if(_transformer != null)
                {
                    _transformer.TransformerChanged -= OnTransformerChanged;
                }

                if(Set(ref _transformer, value, nameof(Transformer)) && (_transformer != null))
                {
                    _transformer.TransformerChanged += OnTransformerChanged;
                }
            } 
        }
        #endregion

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

        private void OnTransformerChanged(object sender, double e)
        {
            Task.Run(()=>
             {
                 var p = _transformer.Transform(new Interfaces.Probing.Point() { X = RelativeX, Y = RelativeY, Z = RelativeZ });

                 X = p.X;
                 Y = p.Y;
                 Z = p.Z;
             });
        }
        #endregion
    }
}
