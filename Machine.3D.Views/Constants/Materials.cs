using HelixToolkit.Wpf.SharpDX;
using Machine._3D.Views.Interfaces;
using Machine.ViewModels.Base;
using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Constants
{
    public class Materials : BaseViewModel, IPanelMaterials
    {
        private Material _panelOuter;
        public Material PanelOuter 
        { 
            get => _panelOuter; 
            set => Set(ref _panelOuter, value, nameof(PanelOuter)); 
        }
        private Material _panelInner;
        public Material PanelInner 
        { 
            get => _panelInner; 
            set => Set(ref _panelInner, value, nameof(PanelInner)); 
        }
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
        public Material ToolDimension => PhongMaterials.Yellow;


        public Materials()
        {
            Machine.ViewModels.Ioc.SimpleIoc<IPanelMaterials>.Register(this);
        }
    }
}
