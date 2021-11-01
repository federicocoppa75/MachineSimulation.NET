using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Probing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    class ProbeViewModel : BaseViewModel
    {
        public int Id { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public IProbe Probe { get; private set; }

        public ProbeViewModel(IProbe probe) : base()
        {
            Probe = probe;
        }
    }
}
