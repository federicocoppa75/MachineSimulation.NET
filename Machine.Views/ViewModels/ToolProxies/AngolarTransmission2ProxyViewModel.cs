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
    internal class AngolarTransmission2ProxyViewModel : AngolarTransmission1ProxyViewModel
    {
        [Category("Sub spindles")]
        [PropertyOrder(2)]
        [ExpandableObject]
        public ATSubSpindleProxyViewModel SubSpindle2 { get; private set; }

        public AngolarTransmission2ProxyViewModel(int nSpindles = 2) : base(nSpindles)
        {
            SubSpindle2 = new ATSubSpindleProxyViewModel(AngolarTransmission.SecondSubSpindle(), this);
        }

        public AngolarTransmission2ProxyViewModel(IAngularTransmission at) : base(at)
        {
            SubSpindle2 = new ATSubSpindleProxyViewModel(at.SecondSubSpindle(), this);
        }

        public AngolarTransmission2ProxyViewModel(AngolarTransmission2ProxyViewModel src) : this()
        {
            CopyFrom(src);
        }

        public override ToolProxyViewModel CreateCopy() => new AngolarTransmission2ProxyViewModel(this);

        protected void CopyFrom(AngolarTransmission2ProxyViewModel src)
        {
            base.CopyFrom(src);
            SubSpindle2.CopyFrom(src.SubSpindle2);
        }
    }
}
