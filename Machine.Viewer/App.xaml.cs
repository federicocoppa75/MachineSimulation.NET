using Machine.Viewer.Helpers;
using Machine.ViewModels;
using System.Windows;
using MVMUI = Machine.ViewModels.UI;
using MDFJ = Machine.DataSource.File.Json;
using MDCR = Machine.DataSource.Client.Rest;
using MVUI = Machine.Views.UI;
using MW32 = Microsoft.Win32;
using M3DGPI = Machine._3D.Geometry.Provider.Interfaces;
using M3DGPIM = Machine._3D.Geometry.Provider.Implementation;
using MSFM = Machine.StepsSource.File.Msteps;
using MVMI = Machine.ViewModels.Interfaces;
using MSVMI = Machine.Steps.ViewModels.Interfaces;
using MSVME = Machine.Steps.ViewModels.Extensions;
using MVMB = Machine.ViewModels.Base;
using MRVM3D = MaterialRemove.ViewModels._3D;
using MRVMI = MaterialRemove.ViewModels.Interfaces;
using MVMIF = Machine.ViewModels.Interfaces.Factories;
using MRMB = MaterialRemove.Machine.Bridge;

namespace Machine.Viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ViewModels.Ioc.SimpleIoc<IKernelViewModel>.Register<KernelViewModel>();
            ViewModels.Ioc.SimpleIoc<MVMB.IMessenger>.Register<MessengerImplementation>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDFJ.DataSource>("File.JSON");
            ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDCR.DataSource>("Client.REST");
            ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.OpenFileDialog>>("OpenFile");
            ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.SaveFileDialog>>("SaveFile");
            ViewModels.Ioc.SimpleIoc<MVMUI.IOptionProvider>.Register(new MVMUI.RegisteredOptionProvider<MVMUI.IDataSource>() { Name = "DataSource" });
            ViewModels.Ioc.SimpleIoc<M3DGPI.IStreamProvider>.Register<M3DGPIM.StlFileStreamProvider>("File.JSON");
            ViewModels.Ioc.SimpleIoc<M3DGPI.IStreamProvider>.Register<M3DGPIM.RestApiStreamProvider>("Client.REST");
            ViewModels.Ioc.SimpleIoc<MVMUI.IListDialog>.Register<MVUI.ListDialog>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IStepsSource>.Register<MSFM.StepsSource>();
            ViewModels.Ioc.SimpleIoc<MSVMI.IDurationProvider>.Register<MSVME.DurationProvider>();
            ViewModels.Ioc.SimpleIoc<MSVMI.IBackStepActionFactory>.Register<MSVME.BackStepActionFactory>();
            ViewModels.Ioc.SimpleIoc<MSVMI.IActionExecuter>.Register<MSVME.ActionExecuter>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IDispatcherHelper>.Register<MVUI.DispatcherHelper>();
            ViewModels.Ioc.SimpleIoc<MVMI.Links.ILinkMovementManager>.Register<MSVME.LinkMovementManager>();
            ViewModels.Ioc.SimpleIoc<MRVMI.IElementViewModelFactory>.Register<MRVM3D.ElementViewModelFactory>();
            ViewModels.Ioc.SimpleIoc<MVMIF.IPanelElementFactory>.Register<MRMB.PanelViewModelFactory>();
            ViewModels.Ioc.SimpleIoc<MVMI.Tools.IToolObserverProvider>.Register<MRMB.ToolsObserverProvider>();
        }
    }
}
