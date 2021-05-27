using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    class GantryLinksViewModel : BaseViewModel
    {
        public IKernelViewModel Kernel { get; private set; }
        public ObservableCollection<GantryLinkViewModel> Links { get; private set; } = new ObservableCollection<GantryLinkViewModel>();

        public GantryLinksViewModel() : base()
        {
            Kernel = Machine.ViewModels.Ioc.SimpleIoc<IKernelViewModel>.GetInstance();

            if (Kernel.Machines is INotifyCollectionChanged ncc) ncc.CollectionChanged += OnMachineCollectionChanged;
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

                if ((evm.LinkToParent != null) && (evm.LinkToParent.MoveType == Data.Enums.LinkMoveType.Linear))
                {
                    var vm = new GantryLinkViewModel() { Master = evm.LinkToParent.Id };

                    vm.Initialize();
                    Links.Add(vm);
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

                if ((evm.LinkToParent != null) && (evm.LinkToParent.MoveType == Data.Enums.LinkMoveType.Linear))
                {
                    var o = Links.FirstOrDefault(g => g.Master == evm.LinkToParent.Id);
                    if (o != null) Links.Remove(o);
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
