using System.Windows;
using Machine.ViewModels;
using MVMUI = Machine.ViewModels.UI;
using MDFJ = Machine.DataSource.File.Json;
using MDCR = Machine.DataSource.Client.Rest;
using MVUI = Machine.Views.UI;
using MW32 = Microsoft.Win32;
using M3DGPI = Machine._3D.Geometry.Provider.Interfaces;
using M3DGPIM = Machine._3D.Geometry.Provider.Implementation;
using MVMI = Machine.ViewModels.Interfaces;
using MVMII = Machine.ViewModels.Interfaces.Insertions;
using MVMIns = Machine.ViewModels.Insertions;
using MVMM = Machine.ViewModels.Messaging;

namespace Machine.Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ViewModels.Ioc.SimpleIoc<MVMI.IKernelViewModel>.Register<KernelViewModel>();
            ViewModels.Ioc.SimpleIoc<MVMM.IMessenger>.Register<MVMM.Messenger>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDFJ.DataSource>("File.JSON");
            ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDCR.DataSource>("Client.REST");
            ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.OpenFileDialog>>("OpenFile");
            ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.SaveFileDialog>>("SaveFile");
            ViewModels.Ioc.SimpleIoc<MVMUI.IOptionProvider>.Register(new MVMUI.RegisteredOptionProvider<MVMUI.IDataSource>() { Name = "DataSource" });
            ViewModels.Ioc.SimpleIoc<M3DGPI.IStreamProvider>.Register<M3DGPIM.StlFileStreamProvider>("File.JSON");
            ViewModels.Ioc.SimpleIoc<M3DGPI.IStreamProvider>.Register<M3DGPIM.RestApiStreamProvider>("Client.REST");
            ViewModels.Ioc.SimpleIoc<MVMUI.IListDialog>.Register<MVUI.ListDialog>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IDispatcherHelper>.Register<MVUI.DispatcherHelper>();
            ViewModels.Ioc.SimpleIoc<MVMII.IInsertionsSinkProvider>.Register<MVMIns.InsertionsSinkProvider>();
            ViewModels.Ioc.SimpleIoc<MVMI.Probing.IProbeFactory>.Register<ViewModels.Probing.ProbeFactory>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IStepsExecutionController>.Register<MVMUI.StepsExecutionControllerStub>();
        }
    }
}
