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
using MVMI = Machine.ViewModels.Interfaces;
using MVMM = Machine.ViewModels.Messaging;
using MVML = Machine.ViewModels.Links;
using MDIF = Machine.Data.Interfaces.Factories;
using MVMB = Machine.ViewModels.Base;

namespace Tooling.Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            Machine.ViewModels.Ioc.SimpleIoc<MVMI.IKernelViewModel>.Register<KernelViewModel>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMM.IMessenger>.Register<MVMM.Messenger>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDFJ.DataSource>("File.JSON");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDFX.DataSource>("File.XML");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDCR.DataSource>("Client.REST");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.OpenFileDialog>>("OpenFile");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.SaveFileDialog>>("SaveFile");
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IOptionProvider>.Register(new MVMUI.RegisteredOptionProvider<MVMUI.IDataSource>() { Name = "DataSource" });
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IApplicationInformationProvider>.Register(new MVMUI.ApplicationInformationProvider(MVMUI.ApplicationType.ToolingEditor));
            Machine.ViewModels.Ioc.SimpleIoc<MVMB.ICommandExceptionObserver>.Register<MVUI.SimpleCommandExceptionObserver>();
            Machine.ViewModels.Ioc.SimpleIoc<MVMUI.IExceptionObserver>.Register<MVUI.SimpleExceptionObserver>();

        }
    }
}
