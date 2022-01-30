using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Interfaces
{
    internal interface IPanelMaterials
    {
        public Material PanelOuter { get; set; }
        public Material PanelInner { get; set; }
    }
}
