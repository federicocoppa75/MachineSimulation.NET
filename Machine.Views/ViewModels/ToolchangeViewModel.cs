using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.MachineElements;
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
    class ToolchangeViewModel : BaseElementsCollectionViewModel
    {
        public ObservableCollection<ToolSinkViewModel> ToolSinks { get; set; } = new ObservableCollection<ToolSinkViewModel>();


        public ToolchangeViewModel() : base()
        {
        }

        protected override void AddElement(IEnumerable<IMachineElement> elements)
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

        protected override void RemoveElement(IEnumerable<IMachineElement> elements)
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

        protected override void Clear() => ToolSinks.Clear();
    }
}
