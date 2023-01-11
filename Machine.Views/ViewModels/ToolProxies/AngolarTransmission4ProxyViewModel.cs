using Machine.Data.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.ToolProxies
{
    internal class AngolarTransmission4ProxyViewModel : AngolarTransmission3ProxyViewModel
    {
        [Category("Sub spindles")]
        [PropertyOrder(4)]
        [ExpandableObject]
        public ATSubSpindleProxyViewModel SubSpindle4 { get; private set; }

        public AngolarTransmission4ProxyViewModel(int nSpindles = 4) : base(nSpindles)
        {
            SubSpindle4 = new ATSubSpindleProxyViewModel(AngolarTransmission.FourthSubSpindle(), this);
        }

        public AngolarTransmission4ProxyViewModel(IAngularTransmission at) : base(at)
        {
            SubSpindle4 = new ATSubSpindleProxyViewModel(at.FourthSubSpindle(), this);
        }

        public AngolarTransmission4ProxyViewModel(AngolarTransmission4ProxyViewModel src) : this()
        {
            CopyFrom(src);
        }

        public override ToolProxyViewModel CreateCopy() => new AngolarTransmission4ProxyViewModel(this);

        protected void CopyFrom(AngolarTransmission4ProxyViewModel src)
        {
            base.CopyFrom(src);
            SubSpindle4.CopyFrom(src.SubSpindle4);
        }
    }
}
