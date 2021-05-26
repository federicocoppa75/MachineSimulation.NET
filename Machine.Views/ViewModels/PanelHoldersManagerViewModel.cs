using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.MachineElements;
using Machine.Views.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    class PanelHoldersManagerViewModel : BaseViewModel
    {
        public IKernelViewModel Kernel { get; private set; }
        public ObservableCollection<PanelHolderManagerViewModel> PanelHolders { get; set; } = new ObservableCollection<PanelHolderManagerViewModel>();
        public PanelData PanelData { get; set; } = new PanelData { Length = 800.0, Width = 600.0, Height = 18.0 };

        public PanelHoldersManagerViewModel() : base()
        {
            Kernel = Machine.ViewModels.Ioc.SimpleIoc<IKernelViewModel>.GetInstance();

            if (Kernel.Machines is INotifyCollectionChanged ncc) ncc.CollectionChanged += OnMachineCollectionChanged;

            Messenger.Register<GetPanelDataMessage>(this, (m) => m.SetPanelData(PanelData));
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
                    PanelHolders.Clear();
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
                if(item is PanelHolderElementViewModel ph)
                {
                    PanelHolders.Add(new PanelHolderManagerViewModel() 
                    { 
                        Id = ph.PanelHolderId,
                        Name = ph.Name,
                        Corner = ph.Corner
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
                if (item is PanelHolderElementViewModel ph)
                {
                    var vm = PanelHolders.FirstOrDefault(p => p.Id == ph.PanelHolderId);

                    if (vm != null) PanelHolders.Remove(vm);
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
