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
    class LinksViewModel : BaseViewModel
    {
        public IKernelViewModel Kernel { get; private set; }
        public ObservableCollection<LinkViewModel> Links { get; private set; } = new ObservableCollection<LinkViewModel>();

        public LinksViewModel() : base()
        {
            Kernel = Machine.ViewModels.Ioc.SimpleIoc<IKernelViewModel>.GetInstance();

            if(Kernel.Machines is INotifyCollectionChanged ncc) ncc.CollectionChanged += OnMachineCollectionChanged;
        }

        private void OnMachineCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddElement(sender, e);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveElement(sender, e);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceElement(sender, e);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Links.Clear();
                    break;
                default:
                    break;
            }
        }

        private void AddElement(object sender, NotifyCollectionChangedEventArgs e)
        {
            AddElement(e.NewItems.Cast<ElementViewModel>());
        }

        private void AddElement(IEnumerable<ElementViewModel> elements)
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

        private void RemoveElement(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveElement(e.OldItems.Cast<ElementViewModel>());
        }

        private void RemoveElement(IEnumerable<ElementViewModel> elements)
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

        private void ReplaceElement(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveElement(sender, e);
            AddElement(sender, e);
        }
    }
}
