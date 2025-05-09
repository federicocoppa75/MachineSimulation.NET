﻿using System;
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
//using M3DVI = Machine._3D.Views.Interfaces;
using MVH = Machine.Views.Helpers;

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

            Properties.Settings.Default.BackgroundColorStart = MVH.MainWindowHelper.Convert(vm.BackgroundColor.Start);
            Properties.Settings.Default.BackgroundColorStop = MVH.MainWindowHelper.Convert(vm.BackgroundColor.Stop);
            Properties.Settings.Default.LightType = vm.LightType.Value.ToString();
            Properties.Settings.Default.View3DFlags = MVH.MainWindowHelper.Convert(vm.View3DFlags);
            Properties.Settings.Default.View3DOptions = MVH.MainWindowHelper.Convert(vm.View3DOptions);
            Properties.Settings.Default.DataSource = vm.DataSource.ToString();
            Properties.Settings.Default.AutoStepOver = vm.StepsExecutionController.AutoStepOver;
            Properties.Settings.Default.DynamicTransition = vm.StepsExecutionController.DynamicTransition;
            Properties.Settings.Default.TimespanFactor = vm.StepsExecutionController.TimeSpanFactor.ToString();
            Properties.Settings.Default.MaterialRemove = vm.MaterialRemoveData.Enable;
            Properties.Settings.Default.ProbeSize = vm.ProbeSize.Value.ToString();
            Properties.Settings.Default.ProbeColor = vm.ProbeColor.Value.ToString();
            Properties.Settings.Default.ProbeShape = vm.ProbeShape.Value.ToString();
            Properties.Settings.Default.MinimumSampleTime = vm.StepsExecutionController.MinimumSampleTime.ToString();
            Properties.Settings.Default.PanelOuterMaterial = vm.PanelOuterMaterial.Value;
            Properties.Settings.Default.PanelInnerMaterial = vm.PanelInnerMaterial.Value;
            Properties.Settings.Default.PanelFragmentType = vm.MaterialRemoveData.PanelFragment.ToString();
            Properties.Settings.Default.SectionDivision = vm.MaterialRemoveData.SectionDivision.ToString();
            Properties.Settings.Default.InnerPanelWireframe = vm.PanelWireframe.Inner;
            Properties.Settings.Default.OuterPanelWireframe = vm.PanelWireframe.Outer;
        }

        private void UpdateFromSettings()
        {
            var vm = DataContext as MainViewModel;

            vm.BackgroundColor.Start = MVH.MainWindowHelper.Convert(Properties.Settings.Default.BackgroundColorStart);
            vm.BackgroundColor.Stop = MVH.MainWindowHelper.Convert(Properties.Settings.Default.BackgroundColorStop);
            vm.LightType.TryToParse(Properties.Settings.Default.LightType);
            MVH.MainWindowHelper.TryToParse(Properties.Settings.Default.View3DFlags, vm.View3DFlags);
            MVH.MainWindowHelper.TryToParse(Properties.Settings.Default.View3DOptions, vm.View3DOptions);
            vm.DataSource.TryToParse(Properties.Settings.Default.DataSource);
            vm.StepsExecutionController.AutoStepOver = Properties.Settings.Default.AutoStepOver;
            vm.StepsExecutionController.DynamicTransition = Properties.Settings.Default.DynamicTransition;
            vm.TimespanFactor.TryToParse(Properties.Settings.Default.TimespanFactor);
            vm.MaterialRemoveData.Enable = Properties.Settings.Default.MaterialRemove;
            vm.ProbeSize.TryToParse(Properties.Settings.Default.ProbeSize);
            vm.ProbeColor.TryToParse(Properties.Settings.Default.ProbeColor);
            vm.ProbeShape.TryToParse(Properties.Settings.Default.ProbeShape);
            vm.SampleTimeOptions.TryToParse(Properties.Settings.Default.MinimumSampleTime);
            vm.PanelOuterMaterial.TryToParse(Properties.Settings.Default.PanelOuterMaterial);
            vm.PanelInnerMaterial.TryToParse(Properties.Settings.Default.PanelInnerMaterial);
            vm.PanelFragmentOptions.TryToParse(Properties.Settings.Default.PanelFragmentType);
            vm.SectionDivisionOptions.TryToParse(Properties.Settings.Default.SectionDivision);
            vm.PanelWireframe.Outer = Properties.Settings.Default.OuterPanelWireframe;
            vm.PanelWireframe.Inner = Properties.Settings.Default.InnerPanelWireframe;
        }
    }
}
