using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public class IndicatorsViewController : BaseViewModel, IIndicatorsViewController
    {
        private bool _collider;
        public bool Collider 
        { 
            get => _collider;
            set => Set(ref _collider, value, nameof(Collider));
        }

        private bool _panelHolder;
        public bool PanelHolder 
        { 
            get => _panelHolder; 
            set => Set(ref _panelHolder, value, nameof(PanelHolder)); 
        }

        private bool _toolHolder;
        public bool ToolHolder 
        { 
            get => _toolHolder; 
            set => Set(ref _toolHolder, value, nameof(ToolHolder)); 
        }

        private bool _inserter;
        public bool Inserter 
        { 
            get => _inserter; 
            set => Set(ref _inserter, value, nameof(Inserter)); 
        }
    }
}
