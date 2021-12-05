using System.Collections.Generic;
using M3DVE = Machine._3D.Views.Enums;
using VMUI = Machine.ViewModels.UI;
using M3DVI = Machine._3D.Views.Interfaces;
using MVMUI = Machine.ViewModels.UI;

namespace Machine.Editor
{
    class MainViewModel : Machine.ViewModels.MainViewModel
    {
        public M3DVI.IBackgroundColor BackgroundColor => ViewModels.Ioc.SimpleIoc<M3DVI.IBackgroundColor>.GetInstance();
        public VMUI.IOptionProvider<M3DVE.LightType> LightType => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<M3DVE.LightType>>.GetInstance();
        public ICollection<VMUI.IFlag> View3DFlags => ViewModels.Ioc.SimpleIoc<VMUI.IPeropertiesProvider>.GetInstance().Flags;
        public ICollection<VMUI.IOptionProvider> View3DOptions => ViewModels.Ioc.SimpleIoc<VMUI.IPeropertiesProvider>.GetInstance().Options;
        public VMUI.IOptionProvider DataSource => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider>.GetInstance();
        public VMUI.IProbesController ProbesController => ViewModels.Ioc.SimpleIoc<VMUI.IProbesController>.GetInstance();
        public VMUI.IOptionProvider<M3DVE.ProbeSize> ProbeSize => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<M3DVE.ProbeSize>>.GetInstance();
        public VMUI.IOptionProvider<M3DVE.ProbeColor> ProbeColor => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<M3DVE.ProbeColor>>.GetInstance();
        public VMUI.IOptionProvider<M3DVE.ProbeShape> ProbeShape => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<M3DVE.ProbeShape>>.GetInstance();
        public VMUI.IMachineStructEditor StructEditor => ViewModels.Ioc.SimpleIoc<VMUI.IMachineStructEditor>.GetInstance();
        public MVMUI.IIndicatorsViewController IndicatorsController => ViewModels.Ioc.SimpleIoc<MVMUI.IIndicatorsViewController>.GetInstance();

        public MainViewModel()
        {

        }
    }
}
