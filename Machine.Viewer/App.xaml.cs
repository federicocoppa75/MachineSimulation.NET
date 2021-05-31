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
            ViewModels.Ioc.SimpleIoc<MVMI.IMessenger>.Register<MessengerImplementation>();
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
        }
    }
}
