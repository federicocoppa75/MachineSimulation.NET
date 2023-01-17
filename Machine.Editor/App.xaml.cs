using System.Windows;
using Machine.ViewModels;
using MVMUI = Machine.ViewModels.UI;
using MDFJ = Machine.DataSource.File.Json;
using MDFX = Machine.DataSource.File.Xml;
using MDCR = Machine.DataSource.Client.Rest;
using MVUI = Machine.Views.UI;
using MW32 = Microsoft.Win32;
using M3DGPI = Machine._3D.Geometry.Provider.Interfaces;
using M3DGPIM = Machine._3D.Geometry.Provider.Implementation;
using MVMI = Machine.ViewModels.Interfaces;
using MVMII = Machine.ViewModels.Interfaces.Insertions;
using MVMIns = Machine.ViewModels.Insertions;
using MVMM = Machine.ViewModels.Messaging;
using MVMIF = Machine.ViewModels.Interfaces.Factories;
using MVMF = Machine.ViewModels.Factories;
using MVML = Machine.ViewModels.Links;
using MVMB = Machine.ViewModels.Base;
using MVMBI = Machine.ViewModels.Base.Implementation;

namespace Machine.Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ViewModels.Ioc.SimpleIoc<MVMI.IKernelViewModel>.Register<MVMBI.KernelViewModel>();
            ViewModels.Ioc.SimpleIoc<MVMM.IMessenger>.Register<MVMM.Messenger>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDFJ.DataSource>("File.JSON");
            ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDFX.DataSource>("File.XML");
            ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDCR.DataSource>("Client.REST");
            ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.OpenFileDialog>>("OpenFile");
            ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.SaveFileDialog>>("SaveFile");
            ViewModels.Ioc.SimpleIoc<MVMUI.IOptionProvider>.Register(new MVMUI.RegisteredOptionProvider<MVMUI.IDataSource>() { Name = "DataSource" });
            ViewModels.Ioc.SimpleIoc<M3DGPI.IStreamProvider>.Register<M3DGPIM.StlFileStreamProvider>("File.JSON");
            ViewModels.Ioc.SimpleIoc<M3DGPI.IStreamProvider>.Register<M3DGPIM.StlFileStreamProvider>("File.XML");
            ViewModels.Ioc.SimpleIoc<M3DGPI.IStreamProvider>.Register<M3DGPIM.RestApiStreamProvider>("Client.REST");
            ViewModels.Ioc.SimpleIoc<MVMUI.IListDialog>.Register<MVUI.ListDialog>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IDispatcherHelper>.Register<MVUI.DispatcherHelper>();
            ViewModels.Ioc.SimpleIoc<MVMII.IInsertionsSinkProvider>.Register<MVMIns.InsertionsSinkProvider>();
            ViewModels.Ioc.SimpleIoc<MVMI.Probing.IProbeFactory>.Register<ViewModels.Probing.ProbeFactory>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IStepsExecutionController>.Register<MVMUI.StepsExecutionControllerStub>();
            ViewModels.Ioc.SimpleIoc<MVMIF.IMachineElementFactoriesProvider>.Register<MVMF.MachineElementFactoriesProvider>();
            ViewModels.Ioc.SimpleIoc<MVMIF.ILinkFactoriesProvider>.Register<MVMF.LinkFactoriesProvider>();
            ViewModels.Ioc.SimpleIoc<MVMI.Links.ILinkMovementController>.Register<MVML.LinkMovementControllerStub>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IApplicationInformationProvider>.Register(new MVMUI.ApplicationInformationProvider(MVMUI.ApplicationType.MachineEditor));
            ViewModels.Ioc.SimpleIoc<MVMUI.IIndicatorsViewController>.Register<MVMUI.IndicatorsViewController>();
            ViewModels.Ioc.SimpleIoc<MVMB.ICommandExceptionObserver>.Register<MVUI.SimpleCommandExceptionObserver>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IExceptionObserver>.Register<MVUI.SimpleExceptionObserver>();
            ViewModels.Ioc.SimpleIoc<MVMIF.IHandleFactory>.Register<MVMF.HandleFactory>();
        }
    }
}
