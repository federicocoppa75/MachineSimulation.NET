

using Machine.ViewModels.Base;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Machine._3D.Views.ViewModels.Lights
{
    public class DirectionalLightViewModel : BaseViewModel
    {
        private Color _color;
        public Color Color
        {
            get => _color;
            set => Set(ref _color, value, nameof(Color));
        }

        public Vector3D Direction { get; set; }
    }
}
