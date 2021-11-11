using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Enums
{
    public enum ElementViewType
    {
        Default,
        XLinearTranslateEle,
        YLinearTranslateEle,
        ZLinearTranslateEle,
        XPneumaticTranslateEle,
        YPneumaticTranslateEle,
        ZPneumaticTranslateEle,
        LinearRotaryEle,
        PneumaticRotatyEle,
        PanelHandler,
        Panel,
        PanelHolder,
        PanelHooker,
        PointDistance,
        ToolEle,
        ToolHolderEle,
        ToolHolderEleZPneu,
        ToolHolderEleRotary,
        AngularTransmission,
        ATToolholder,
        SectionedPanel,
        Debug,
        InjectedObj,
        InsertedObj,
        PointProbe,
        DistanceProbe
    }
}
