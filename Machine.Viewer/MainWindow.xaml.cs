using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;
using Machine.ViewModels;
using VMUI = Machine.ViewModels.UI;
using MVUI = Machine.Views.UI;

namespace Machine.Viewer
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

        private void MenuItem_Debug_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            SaveToSettings();
            Properties.Settings.Default.Save();
        }

        private void SaveToSettings()
        {
            var vm = DataContext as MainViewModel;

            Properties.Settings.Default.BackgroundColorStart = Convert(vm.BackgroundColor.Start);
            Properties.Settings.Default.BackgroundColorStop = Convert(vm.BackgroundColor.Stop);
            Properties.Settings.Default.LightType = vm.LightType.Value.ToString();
            Properties.Settings.Default.View3DFlags = Convert(vm.View3DFlags);
            Properties.Settings.Default.View3DOptions = Convert(vm.View3DOptions);
            Properties.Settings.Default.DataSource = vm.DataSource.ToString();
            Properties.Settings.Default.AutoStepOver = vm.StepsExecutionController.AutoStepOver;
            Properties.Settings.Default.DynamicTransition = vm.StepsExecutionController.DynamicTransition;
            Properties.Settings.Default.TimespanFactor = vm.StepsExecutionController.TimeSpanFactor.ToString();
            Properties.Settings.Default.MaterialRemove = vm.MaterialRemoveData.Enable;
        }

        private void UpdateFromSettings()
        {
            var vm = DataContext as MainViewModel;

            vm.BackgroundColor.Start = Convert(Properties.Settings.Default.BackgroundColorStart);
            vm.BackgroundColor.Stop = Convert(Properties.Settings.Default.BackgroundColorStop);
            vm.LightType.TryToParse(Properties.Settings.Default.LightType);
            TryToParse(Properties.Settings.Default.View3DFlags, vm.View3DFlags);
            TryToParse(Properties.Settings.Default.View3DOptions, vm.View3DOptions);
            vm.DataSource.TryToParse(Properties.Settings.Default.DataSource);
            vm.StepsExecutionController.AutoStepOver = Properties.Settings.Default.AutoStepOver;
            vm.StepsExecutionController.DynamicTransition = Properties.Settings.Default.DynamicTransition;
            vm.TimespanFactor.TryToParse(Properties.Settings.Default.TimespanFactor);
            vm.MaterialRemoveData.Enable = Properties.Settings.Default.MaterialRemove;
        }

        private static MColor Convert(DColor color)
        {
            return new MColor()
            {
                A = color.A,
                B = color.B,
                G = color.G,
                R = color.R,
            };
        }

        private static DColor Convert(MColor color) => DColor.FromArgb(color.A, color.R, color.G, color.B);

        private static string Convert(ICollection<VMUI.IFlag> flags)
        {
            var d = flags.ToDictionary((o) => o.Name, (o) => o.Value.ToString());

            return Newtonsoft.Json.JsonConvert.SerializeObject(d, Newtonsoft.Json.Formatting.Indented);
        }

        private static string Convert(ICollection<VMUI.IOptionProvider> options)
        {
            var d = options.ToDictionary((o) => o.Name, (o) => o.ToString());

            return Newtonsoft.Json.JsonConvert.SerializeObject(d, Newtonsoft.Json.Formatting.Indented);
        }

        private static bool TryToParse(string data, ICollection<VMUI.IFlag> flags)
        {
            var result = true;
            Dictionary<string, string> dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

            foreach (var item in flags)
            {
                if(dictionary.TryGetValue(item.Name, out string value))
                {
                    item.TryToParse(value);
                }
            }

            return result;
        }

        private static bool TryToParse(string data, ICollection<VMUI.IOptionProvider> options)
        {
            var result = true;
            Dictionary<string, string> dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

            foreach (var item in options)
            {
                if (dictionary.TryGetValue(item.Name, out string value))
                {
                    item.TryToParse(value);
                }
            }

            return result;
        }
    }
}
