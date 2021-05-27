using Machine.Data.Converters;
using Machine.Data.MachineElements;
using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.MachineElements;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using MDTooling = Machine.Data.Toolings;
using MDTools = Machine.Data.Tools;
using Machine.ViewModels.Helpers;
using System.Linq;
using Machine.ViewModels.Messages;
using M3DVE = Machine._3D.Views.Enums;
using VMUI = Machine.ViewModels.UI;
using M3DVI = Machine._3D.Views.Interfaces;

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

        public MainViewModel() : base()
        {
        }
    }
}
