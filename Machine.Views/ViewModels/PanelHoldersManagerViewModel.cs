using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.MachineElements;
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
    class PanelHoldersManagerViewModel : BaseElementsCollectionViewModel
    {
        public ObservableCollection<PanelHolderManagerViewModel> PanelHolders { get; set; } = new ObservableCollection<PanelHolderManagerViewModel>();
        public PanelData PanelData { get; set; } = new PanelData { Length = 800.0, Width = 600.0, Height = 18.0 };

        public PanelHoldersManagerViewModel() : base()
        {
            Messenger.Register<GetPanelDataMessage>(this, (m) => m.SetPanelData(PanelData));
        }

        protected override void AddElement(IEnumerable<IMachineElement> elements)
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

        protected override void RemoveElement(IEnumerable<IMachineElement> elements)
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

        protected override void Clear() => PanelHolders.Clear();
    }
}
