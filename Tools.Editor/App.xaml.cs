using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
using MDF = Machine.Data.Factories;
using MDIF = Machine.Data.Interfaces.Factories;


namespace Tools.Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Machine.ViewModels.Ioc.SimpleIoc<MVMI.IKernelViewModel>.Register<KernelViewModel>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMM.IMessenger>.Register<MVMM.Messenger>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDFJ.DataSource>("File.JSON");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDFX.DataSource>("File.XML");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDCR.DataSource>("Client.REST");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.OpenFileDialog>>("OpenFile");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.SaveFileDialog>>("SaveFile");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IOptionProvider>.Register(new MVMUI.RegisteredOptionProvider<MVMUI.IDataSource>() { Name = "DataSource" });
            Machine.ViewModels.Ioc.SimpleIoc<M3DGPI.IStreamProvider>.Register<M3DGPIM.StlFileStreamProvider>("File.JSON");
            Machine.ViewModels.Ioc.SimpleIoc<M3DGPI.IStreamProvider>.Register<M3DGPIM.StlFileStreamProvider>("File.XML");
            Machine.ViewModels.Ioc.SimpleIoc<M3DGPI.IStreamProvider>.Register<M3DGPIM.RestApiStreamProvider>("Client.REST");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IListDialog>.Register<MVUI.ListDialog>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IDispatcherHelper>.Register<MVUI.DispatcherHelper>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IApplicationInformationProvider>.Register<ApplicationInformationProvider>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IIndicatorsViewController>.Register<MVMUI.IndicatorsViewController>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IStepsExecutionController>.Register<MVMUI.StepsExecutionControllerStub>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMI.Links.ILinkMovementController>.Register<MVML.LinkMovementControllerStub>();
            Machine.ViewModels.Ioc.SimpleIoc<MDIF.IToolFactory>.Register<MDF.ToolFactory>();
        }
    }
}
