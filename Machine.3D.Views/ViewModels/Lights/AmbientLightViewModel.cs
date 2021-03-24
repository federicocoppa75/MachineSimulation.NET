

using Machine.ViewModels.Base;
using System.Windows.Media;

namespace Machine._3D.Views.ViewModels.Lights
{
    public class AmbientLightViewModel : BaseViewModel
    {
        private Color _color;
        public Color Color 
        { 
            get => _color; 
            set => Set(ref _color, value, nameof(Color)); 
        }
    }
}
