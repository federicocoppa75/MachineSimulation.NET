using Machine.Data.Enums;
using Machine.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Machine.Views.Selectors
{
    [ContentProperty("Templates")]
    public class LinkValueChangerTemplateSelector : DataTemplateSelector
    {

        public List<LinkValueChangerTemplateSelectorOptions> Templates { get; set; } = new List<LinkValueChangerTemplateSelectorOptions>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate dt = null;

            if (item is LinkViewModel link)
            {
                foreach (var t in Templates)
                {
                    if (t.When == link.MoveType)
                    {
                        dt = t.Then;
                        break;
                    }
                }
            }

            return dt;
        }
    }

    [ContentProperty("Then")]
    public class LinkValueChangerTemplateSelectorOptions
    {
        public LinkMoveType When { get; set; }
        public DataTemplate Then { get; set; }
    }

}
