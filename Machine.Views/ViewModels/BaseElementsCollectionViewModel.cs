using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    abstract class BaseElementsCollectionViewModel : BaseViewModel
    {
        public IKernelViewModel Kernel { get; private set; }

        public BaseElementsCollectionViewModel() : base()
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
                    Clear(sender, e);
                    break;
                default:
                    break;
            }
        }

        private void AddElement(object sender, NotifyCollectionChangedEventArgs e) => AddElement(e.NewItems.Cast<IMachineElement>());
 
        private void RemoveElement(object sender, NotifyCollectionChangedEventArgs e) => RemoveElement(e.OldItems.Cast<IMachineElement>());

        private void ReplaceElement(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveElement(sender, e);
            AddElement(sender, e);
        }

        private void Clear(object sender, NotifyCollectionChangedEventArgs e) => Clear();

        protected abstract void AddElement(IEnumerable<IMachineElement> elements);
        protected abstract void RemoveElement(IEnumerable<IMachineElement> elements);
        protected abstract void Clear();

    }
}
