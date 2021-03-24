using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Machine._3D.Views.Interfaces
{
    public interface IBackgroundColor
    {
        Color Start { get; set; }
        Color Stop { get; set; }
    }
}
