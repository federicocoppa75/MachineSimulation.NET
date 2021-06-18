using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
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
        public ObservableCollection<ILinkViewModel> Links { get; private set; } = new ObservableCollection<ILinkViewModel>();

        public LinksViewModel() : base()
        {
        }

        protected override void AddElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var item in elements)
            {
                //var evm = item as ElementViewModel;

                if (item.LinkToParent != null)
                {
                    Links.Add(item.LinkToParent);
                }

                AddElement(item.Children);
            }
        }

        protected override void RemoveElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var item in elements)
            {
                //var evm = item as ElementViewModel;

                if (item.LinkToParent != null)
                {
                    Links.Remove(item.LinkToParent);
                }
            }
        }

        protected override void Clear() => Links.Clear();
    }
}
