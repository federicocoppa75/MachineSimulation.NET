using Machine.ViewModels.UI;
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
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Machine.Views
{
    /// <summary>
    /// Interaction logic for PropertyGridFilePicker.xaml
    /// </summary>
    public partial class PropertyGridFilePicker : UserControl, ITypeEditor
    {
        public PropertyGridFilePicker()
        {
            InitializeComponent();
        }



        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(PropertyGridFilePicker), new PropertyMetadata(null));



        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            Binding binding = new Binding("Value");
            binding.Source = propertyItem;
            binding.Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            BindingOperations.SetBinding(this, ValueProperty, binding);
            return this;
        }

        private void PickFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = Machine.ViewModels.Ioc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "stl";
            dlg.Filter = "3D model files (*.3ds;*.obj;*.lwo;*.stl)|*.3ds;*.obj;*.objz;*.lwo;*.stl";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                Value = dlg.FileName;
            }
        }
    }
}
