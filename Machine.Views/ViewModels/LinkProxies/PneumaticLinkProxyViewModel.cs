using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.LinkProxies
{
    public class PneumaticLinkProxyViewModel : LinkProxyViewModel
    {
        private IPneumaticLinkViewModel PneumaticLink => _link as IPneumaticLinkViewModel;

        [PropertyOrder(5)]
        public double OffPos
        {
            get => PneumaticLink.OffPos;
            set => PneumaticLink.OffPos = value;
        }

        [PropertyOrder(6)]
        public double OnPos
        {
            get => PneumaticLink.OnPos;
            set => PneumaticLink.OnPos = value;
        }

        [PropertyOrder(7)]
        public double TOff
        {
            get => PneumaticLink.TOff;
            set => PneumaticLink.TOff = value;
        }

        [PropertyOrder(8)]
        public double TOn
        {
            get => PneumaticLink.TOn;
            set => PneumaticLink.TOn = value;
        }

        [PropertyOrder(9)]
        public bool ToolActivator
        {
            get => PneumaticLink.ToolActivator;
            set => PneumaticLink.ToolActivator = value;
        }

        public PneumaticLinkProxyViewModel(IPneumaticLinkViewModel link) : base(link)
        {
        }
    }
}
