using Machine.Views.ViewModels;
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
using MVMUI = Machine.ViewModels.UI;
using MVMIoc = Machine.ViewModels.Ioc;
using MSVMI = Machine.Steps.ViewModels.Interfaces;

namespace Machine.Views
{
    /// <summary>
    /// Logica di interazione per Steps.xaml
    /// </summary>
    public partial class Steps : UserControl
    {
        public Steps()
        {
            InitializeComponent();
            var vm = new StepsViewModel();
            DataContext = vm;
            MVMIoc.SimpleIoc<MVMUI.IStepsController>.Register(vm);
            MVMIoc.SimpleIoc<MVMUI.IStepsExecutionController>.Register(vm);
            MVMIoc.SimpleIoc<MSVMI.IStepsContainer>.Register(vm);
        }

        private void OnListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listView.ScrollIntoView(listView.SelectedItem);
        }
    }
}
