using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.Links;
using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Machine.Views.ViewModels
{
    class LinksViewModel : BaseElementsCollectionViewModel
    {
        public ObservableCollection<LinkViewModel> Links { get; private set; } = new ObservableCollection<LinkViewModel>();

        public LinksViewModel() : base()
        {
        }

        protected override void AddElement(IEnumerable<ElementViewModel> elements)
        {
            foreach (var item in elements)
            {
                var evm = item as ElementViewModel;

                if (evm.LinkToParent != null)
                {
                    Links.Add(evm.LinkToParent);
                }

                AddElement(item.Children);
            }
        }

        protected override void RemoveElement(IEnumerable<ElementViewModel> elements)
        {
            foreach (var item in elements)
            {
                var evm = item as ElementViewModel;

                if (evm.LinkToParent != null)
                {
                    Links.Remove(evm.LinkToParent);
                }
            }
        }

        protected override void Clear() => Links.Clear();
    }
}
