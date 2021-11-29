using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Machine.Views.ViewModels.LinkProxies
{
    public class LinearLinkProxyViewModel : LinkProxyViewModel
    {
        private ILinearLinkViewModel LinearLInk => _link as ILinearLinkViewModel;

        [PropertyOrder(5)]
        public double Max
        {
            get => LinearLInk.Max;
            set => LinearLInk.Max = value;
        }

        [PropertyOrder(6)]
        public double Min
        {
            get => LinearLInk.Min;
            set => LinearLInk.Min = value;
        }

        [PropertyOrder(7)]
        public double Pos
        {
            get => LinearLInk.Pos;
            set => LinearLInk.Pos = value;
        }


        public LinearLinkProxyViewModel(ILinearLinkViewModel link) : base(link)
        {
        }
    }
}
