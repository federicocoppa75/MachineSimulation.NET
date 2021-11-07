using Machine._3D.Views.Enums;
using Machine._3D.Views.Interfaces;
using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.ViewModels
{
    public class ProbesViewData : BaseViewModel, IProbesViewData
    {
        private ProbeSize _size;
        public ProbeSize Size
        {
            get => _size;
            set => Set(ref _size, value, nameof(Size));
        }

        private ProbeColor _color;
        public ProbeColor Color
        {
            get => _color;
            set => Set(ref _color, value, nameof(Color));
        }

        private ProbeShape _shape;
        public ProbeShape Shape
        {
            get => _shape;
            set => Set(ref _shape, value, nameof(Shape));
        }

        public IEnumerable<ProbeSize> Sizes => Enum.GetValues(typeof(ProbeSize)).Cast<ProbeSize>();
        public IEnumerable<ProbeColor> Colors => Enum.GetValues(typeof(ProbeColor)).Cast<ProbeColor>();
        public IEnumerable<ProbeShape> Shapes => Enum.GetValues(typeof(ProbeShape)).Cast<ProbeShape>();
    }
}
