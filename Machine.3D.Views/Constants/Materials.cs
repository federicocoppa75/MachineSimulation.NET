using HelixToolkit.Wpf.SharpDX;
using Machine._3D.Views.Interfaces;
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
        public Material Probe
        {
            get
            {
                var color = Machine.ViewModels.Ioc.SimpleIoc<IProbesViewData>.GetInstance().Color;

                switch (color)
                {
                    case Enums.ProbeColor.Yellow:
                        return PhongMaterials.Yellow;
                    case Enums.ProbeColor.Red:
                        return PhongMaterials.Red;
                    case Enums.ProbeColor.Blue:
                        return PhongMaterials.Blue;
                    case Enums.ProbeColor.Green:
                        return PhongMaterials.Green;
                    default:
                        return PhongMaterials.Yellow;
                }
            }
        }
        public Material PointIndicator => PhongMaterials.Blue;
        public Material PointAndDirIndicator => PhongMaterials.Red;
        public Material PointsIndicator => PhongMaterials.Green;
    }
}
