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
    class GantryLinksViewModel : BaseElementsCollectionViewModel
    {
        public ObservableCollection<GantryLinkViewModel> Links { get; private set; } = new ObservableCollection<GantryLinkViewModel>();

        public GantryLinksViewModel() : base()
        {
        }

        protected override void AddElement(IEnumerable<ElementViewModel> elements)
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

        protected override void RemoveElement(IEnumerable<ElementViewModel> elements)
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

        protected override void Clear() => Links.Clear();
    }
}
