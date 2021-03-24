using Machine.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Machine.Views
{
    /// <summary>
    /// Logica di interazione per Struct.xaml
    /// </summary>
    public partial class Struct : UserControl
    {
        public Struct()
        {
            InitializeComponent();
            DataContext = new StructViewModel();
        }
    }
}
