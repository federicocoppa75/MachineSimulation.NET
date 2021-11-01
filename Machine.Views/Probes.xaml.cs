using Machine.Views.ViewModels;
using System.Windows.Controls;

namespace Machine.Views
{
    /// <summary>
    /// Interaction logic for Probes.xaml
    /// </summary>
    public partial class Probes : UserControl
    {
        public Probes()
        {
            InitializeComponent();
            DataContext = new ProbesViewModel();
        }
    }
}
