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
    internal class AngolarTransmission3ProxyViewModel : AngolarTransmission2ProxyViewModel
    {
        [Category("Sub spindles")]
        [PropertyOrder(3)]
        [ExpandableObject]
        public ATSubSpindleProxyViewModel SubSpindle3 { get; private set; }

        public AngolarTransmission3ProxyViewModel(int nSpindles = 3) : base(nSpindles)
        {
            SubSpindle3 = new ATSubSpindleProxyViewModel(AngolarTransmission.ThirdSubSpindle(), this);
        }

        public AngolarTransmission3ProxyViewModel(IAngularTransmission at) : base(at)
        {
            SubSpindle3 = new ATSubSpindleProxyViewModel(at.ThirdSubSpindle(), this);
        }

        public AngolarTransmission3ProxyViewModel(AngolarTransmission3ProxyViewModel src) : this()
        {
            CopyFrom(src);
        }

        public override ToolProxyViewModel CreateCopy() => new AngolarTransmission3ProxyViewModel(this);

        protected void CopyFrom(AngolarTransmission3ProxyViewModel src)
        {
            base.CopyFrom(src);
            SubSpindle3.CopyFrom(src.SubSpindle3);
        }
    }
}
