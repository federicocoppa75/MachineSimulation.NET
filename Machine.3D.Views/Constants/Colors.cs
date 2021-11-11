using Machine._3D.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWM = System.Windows.Media;

namespace Machine._3D.Views.Constants
{
    class Colors
    {
        public SWM.Color Probe 
        { 
            get
            {
                var color = Machine.ViewModels.Ioc.SimpleIoc<IProbesViewData>.GetInstance().Color;

                switch (color)
                {
                    case Enums.ProbeColor.Yellow:
                        return SWM.Colors.Yellow;
                    case Enums.ProbeColor.Red:
                        return SWM.Colors.Red;
                    case Enums.ProbeColor.Blue:
                        return SWM.Colors.Blue;
                    case Enums.ProbeColor.Green:
                        return SWM.Colors.Green;
                    default:
                        return SWM.Colors.Yellow;
                }
            }
        }
    }
}
