﻿using Client.Machine.ViewModels;
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
using MD = Machine.Data;

namespace Client.Machine
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
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).SelectedItem = (sender as TreeViewItem).DataContext as MD.MachineElements.MachineElement;
            e.Handled = true;
        }
    }
}
