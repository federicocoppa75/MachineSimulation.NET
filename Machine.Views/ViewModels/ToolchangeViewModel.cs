using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.MachineElements.Toolholder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    class ToolchangeViewModel : BaseViewModel
    {
        public IKernelViewModel Kernel { get; private set; }
        public ObservableCollection<ToolSinkViewModel> ToolSinks { get; set; } = new ObservableCollection<ToolSinkViewModel>();


        public ToolchangeViewModel() : base()
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
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveElement(sender, e);
                    break;
                case NotifyCollectionChangedAction.Move:
                    ReplaceElement(sender, e);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ToolSinks.Clear();
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
                if (item is AutoSyncToolholderElementViewModel th)
                {
                    ToolSinks.Add(new ToolSinkViewModel()
                    {
                        Id = th.ToolHolderId,
                        Name = th.Name
                    });
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
                if (item is AutoSyncToolholderElementViewModel th)
                {
                    var vm = ToolSinks.FirstOrDefault(p => p.Id == th.ToolHolderId);

                    if (vm != null) ToolSinks.Remove(vm);
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
