using Machine._3D.Views.Interfaces;
using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Machine._3D.Views.ViewModels
{
    public class BackgroundColor : BaseViewModel, IBackgroundColor
    {
        private Color _start;
        public Color Start
        { 
            get => _start; 
            set => Set(ref _start, value, nameof(Start));
        }

        private Color _stop;
        public Color Stop 
        { 
            get => _stop; 
            set => Set(ref _stop, value, nameof(Stop)); 
        }
    }
}
