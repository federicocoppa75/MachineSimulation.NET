using Machine.ViewModels.Base;
using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.ViewModels
{
    internal class PanelWireframeViewModel : BaseViewModel, IPanelWireframe
    {
        private bool _inner;
        public bool Inner 
        { 
            get => _inner; 
            set => Set(ref _inner, value, nameof(Inner)); 
        }

        private bool _outer;
        public bool Outer 
        { 
            get => _outer; 
            set => Set(ref _outer, value, nameof(Outer)); 
        }
    }

    internal class PanelWireframeProxy : BaseViewModel, IPanelWireframe
    {
        IPanelWireframe _panelWireframe;

        public bool Inner 
        { 
            get => _panelWireframe.Inner; 
            set => throw new NotImplementedException(); 
        }
        public bool Outer 
        { 
            get => _panelWireframe.Outer; 
            set => throw new NotImplementedException(); 
        }

        public PanelWireframeProxy()
        {
            _panelWireframe = GetInstance<IPanelWireframe>();
            (_panelWireframe as BaseViewModel).PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RisePropertyChanged(e.PropertyName);
        }
    }
}
