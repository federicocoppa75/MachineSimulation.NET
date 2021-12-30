using Machine.ViewModels.Base;
using Machine.Views.Messages.ToolsEditor;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Machine.Views
{
    /// <summary>
    /// Interaction logic for PropertyGridToolSelectionCombo.xaml
    /// </summary>
    public partial class PropertyGridToolSelectionCombo : UserControl, ITypeEditor
    {
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(PropertyGridToolSelectionCombo), new PropertyMetadata(null));

        public PropertyGridToolSelectionCombo()
        {
            InitializeComponent();
            DataContext = new PropertyGridToolSelectionComboViewModel();
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            Binding binding = new Binding("Value");
            binding.Source = propertyItem;
            binding.Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
            BindingOperations.SetBinding(this, ValueProperty, binding);
            return this;
        }

    }

    internal class PropertyGridToolSelectionComboViewModel : BaseViewModel
    {
        public IEnumerable<string> Tools 
        {
            get
            {
                var list = new List<string>();

                Messenger.Send(new ToolsRequestMessage()
                {
                    SetTools = (tools) =>
                    {
                        if((tools != null) && (tools.Count() > 0))
                        {
                            foreach (var tool in tools)
                            {
                                if(tool.ToolLinkType == Data.Enums.ToolLinkType.Static)
                                {
                                    list.Add(tool.Name);
                                }
                            }

                            if (list.Count() > 0) list.Insert(0, "");
                        }
                    }
                });

                return list;
            }
        }

        public PropertyGridToolSelectionComboViewModel() : base()
        {
        }
    }
}
