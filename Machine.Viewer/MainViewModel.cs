using System.Collections.Generic;
using M3DVE = Machine._3D.Views.Enums;
using VMUI = Machine.ViewModels.UI;
using M3DVI = Machine._3D.Views.Interfaces;
using MRI = MaterialRemove.Interfaces;
using MRIE = MaterialRemove.Interfaces.Enums;
using System;
using System.Linq;
using MVMIoc = Machine.ViewModels.Ioc;

namespace Machine.Viewer
{
    class MainViewModel : Machine.ViewModels.MainViewModel
    {
        public M3DVI.IBackgroundColor BackgroundColor => ViewModels.Ioc.SimpleIoc<M3DVI.IBackgroundColor>.GetInstance();
        public VMUI.IOptionProvider<M3DVE.LightType> LightType => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<M3DVE.LightType>>.GetInstance();
        public ICollection<VMUI.IFlag> View3DFlags => ViewModels.Ioc.SimpleIoc<VMUI.IPeropertiesProvider>.GetInstance().Flags;
        public ICollection<VMUI.IOptionProvider> View3DOptions => ViewModels.Ioc.SimpleIoc<VMUI.IPeropertiesProvider>.GetInstance().Options;
        public VMUI.IOptionProvider DataSource => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider>.GetInstance();
        public VMUI.IStepsController StepController => ViewModels.Ioc.SimpleIoc<VMUI.IStepsController>.GetInstance();
        public VMUI.IStepsExecutionController StepsExecutionController => ViewModels.Ioc.SimpleIoc<VMUI.IStepsExecutionController>.GetInstance();
        public VMUI.IOptionProvider<VMUI.TimeSpanFactor> TimespanFactor => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<VMUI.TimeSpanFactor>>.GetInstance();
        public VMUI.IOptionProvider<VMUI.SampleTimeOption> SampleTimeOptions => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<VMUI.SampleTimeOption>>.GetInstance();
        public MRI.IMaterialRemoveData MaterialRemoveData => ViewModels.Ioc.SimpleIoc<MRI.IMaterialRemoveData>.GetInstance();
        public VMUI.IOptionProvider<MRIE.PanelFragment> PanelFragmentOptions => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<MRIE.PanelFragment>>.GetInstance();
        public VMUI.IOptionProvider<MRIE.SectionDivision> SectionDivisionOptions => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<MRIE.SectionDivision>>.GetInstance();
        public VMUI.IProbesController ProbesController => ViewModels.Ioc.SimpleIoc<VMUI.IProbesController>.GetInstance();
        public VMUI.IOptionProvider<M3DVE.ProbeSize> ProbeSize => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<M3DVE.ProbeSize>>.GetInstance();
        public VMUI.IOptionProvider<M3DVE.ProbeColor> ProbeColor => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<M3DVE.ProbeColor>>.GetInstance();
        public VMUI.IOptionProvider<M3DVE.ProbeShape> ProbeShape => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<M3DVE.ProbeShape>>.GetInstance();
        public MRI.IPanelExportController PanelController => ViewModels.Ioc.SimpleIoc<MRI.IPanelExportController>.GetInstance();
        public VMUI.IOptionProvider<string> PanelOuterMaterial => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<string>>.GetInstance("PanelOuterMaterial");
        public VMUI.IOptionProvider<string> PanelInnerMaterial => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<string>>.GetInstance("PanelInnerMaterial");
        public VMUI.IViewExportController ViewExportController => ViewModels.Ioc.SimpleIoc<VMUI.IViewExportController>.GetInstance();

        public MainViewModel() : base()
        {
            MVMIoc.SimpleIoc<VMUI.IOptionProvider<MRIE.PanelFragment>>
                .Register(new VMUI.EnumOptionProxy<MRIE.PanelFragment>(() => MaterialRemoveData.PanelFragment, 
                                                                  (v) => MaterialRemoveData.PanelFragment = v));

            MVMIoc.SimpleIoc<VMUI.IOptionProvider<MRIE.SectionDivision>>
                .Register(new VMUI.EnumOptionProxy<MRIE.SectionDivision>(() => MaterialRemoveData.SectionDivision,
                                                                    (v) => MaterialRemoveData.SectionDivision = v));
        }
    }
}
