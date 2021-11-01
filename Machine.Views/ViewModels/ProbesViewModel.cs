using Machine.ViewModels;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Probing;
using Machine.ViewModels.Messages.Probing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    class ProbesViewModel : BaseElementsCollectionViewModel
    {
        public ObservableCollection<IProbe> Probes { get; private set; } = new ObservableCollection<IProbe>();

        public ProbesViewModel() : base()
        {
            Messenger.Register<AddProbeMessage>(this, OnAddProbeMessage);
        }

        private void OnAddProbeMessage(AddProbeMessage msg)
        {
            Probes.Add(msg.Probe);
        }

        protected override void AddElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var item in elements)
            {
                if(item is IProbe p)
                {
                    Probes.Add(p);
                }

                AddElement(item.Children);
            }
        }

        protected override void RemoveElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var item in elements)
            {
                if(item is IProbe p)
                {
                    Probes.Remove(p);
                }

                RemoveElement(item.Children);
            }
        }

        protected override void Clear() => Probes.Clear();
    }
}
