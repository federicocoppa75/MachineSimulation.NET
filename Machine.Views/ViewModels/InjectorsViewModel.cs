using Machine.ViewModels;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    class InjectorsViewModel : BaseElementsCollectionViewModel
    {
        public ObservableCollection<InjectorViewModel> Injectors { get; private set; } = new ObservableCollection<InjectorViewModel>();

        protected override void AddElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var item in elements)
            {
                if (item is InjectorElementViewModel inj)
                {
                    Injectors.Add(new InjectorViewModel() { Id = inj.InserterId });
                }

                AddElement(item.Children);
            }
        }

        protected override void RemoveElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var item in elements)
            {
                if (item is InjectorElementViewModel inj)
                {
                    var vm = Injectors.FirstOrDefault(p => p.Id == inj.InserterId);

                    if (vm != null) Injectors.Remove(vm);
                }
            }
        }

        protected override void Clear() => Injectors.Clear();
    }
}
