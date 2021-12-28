using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MVUI = Machine.Views.UI;
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;
using Machine.ViewModels;
using VMUI = Machine.ViewModels.UI;
using MVH = Machine.Views.Helpers;
using System.ComponentModel;

namespace Tools.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            MVUI.DispatcherHelper.Initialize();
            UpdateFromSettings();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            SaveToSettings();
            Settings.Default.Save();
        }

        private void SaveToSettings()
        {
            var vm = DataContext as MainViewModel;

            Settings.Default.BackgroundColorStart = MVH.MainWindowHelper.Convert(vm.BackgroundColor.Start);
            Settings.Default.BackgroundColorStop = MVH.MainWindowHelper.Convert(vm.BackgroundColor.Stop);
            Settings.Default.LightType = vm.LightType.Value.ToString();
            Settings.Default.View3DFlags = MVH.MainWindowHelper.Convert(vm.View3DFlags);
            Settings.Default.View3DOptions = MVH.MainWindowHelper.Convert(vm.View3DOptions);
            Settings.Default.DataSource = vm.DataSource.ToString();
            Settings.Default.ViewToolHolder = vm.IndicatorsController.ToolHolder;
        }

        private void UpdateFromSettings()
        {
            var vm = DataContext as MainViewModel;

            vm.BackgroundColor.Start = MVH.MainWindowHelper.Convert(Settings.Default.BackgroundColorStart);
            vm.BackgroundColor.Stop = MVH.MainWindowHelper.Convert(Settings.Default.BackgroundColorStop);
            vm.LightType.TryToParse(Settings.Default.LightType);
            MVH.MainWindowHelper.TryToParse(Settings.Default.View3DFlags, vm.View3DFlags);
            MVH.MainWindowHelper.TryToParse(Settings.Default.View3DOptions, vm.View3DOptions);
            vm.DataSource.TryToParse(Settings.Default.DataSource);
            vm.IndicatorsController.ToolHolder = Settings.Default.ViewToolHolder;
        }
    }
}
