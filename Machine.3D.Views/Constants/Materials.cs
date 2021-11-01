using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Constants
{
    public class Materials
    {
        public Material Panel => PhongMaterials.Orange;
        public Material PanelIntern => PhongMaterials.Bronze;
        public Material Debug => PhongMaterials.Red;
        public Material Probe => PhongMaterials.Yellow;
    }
}
