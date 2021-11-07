using Machine._3D.Views.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Interfaces
{
    public interface IProbesViewData
    {
        ProbeSize Size { get; set; }
        ProbeColor Color { get; set; }
        ProbeShape Shape { get; set; }
        IEnumerable<ProbeSize> Sizes { get; }
        IEnumerable<ProbeColor> Colors { get; }
        IEnumerable<ProbeShape> Shapes { get; }
    }
}
