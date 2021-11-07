using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Probing;
using Machine.ViewModels.Messages.Probing;
using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Machine.Views.ViewModels
{
    class ProbesViewModel : BaseElementsCollectionViewModel, IProbesController
    {
        public ObservableCollection<IProbe> Probes { get; private set; } = new ObservableCollection<IProbe>();

        #region IProbesController
        private bool _active;
        public bool Active
        {
            get => _active;
            set => Set(ref _active, value, nameof(Active));
        }

        private ICommand _distanceCommand;
        public ICommand DistanceCommand => _distanceCommand ?? (_distanceCommand = new RelayCommand(() => DistanceCommandImpl(), () => _active));

 
        private ICommand _removeCommand;
        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new RelayCommand(() => RemoveCommandImpl(), () => false /*(Probes.Count > 0) && Probes.Any(p => (p is IViewElementData ved) && ved.IsSelected)*/));

        private ICommand _removeAllCommand;
        public ICommand RemoveAllCommand => _removeAllCommand ?? (_removeAllCommand = new RelayCommand(() => RemoveAllCommandImpl(), () => Probes.Count > 0));
        #endregion

        public ProbesViewModel() : base()
        {
            Machine.ViewModels.Ioc.SimpleIoc<IProbesController>.Register(this);
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

        private void RemoveCommandImpl()
        {
            var selected = Probes.Where(p => (p is IViewElementData ved) && ved.IsSelected);

            foreach (var item in selected) Probes.Remove(item);
        }

        private void RemoveAllCommandImpl() => Clear();

        private void DistanceCommandImpl()
        {
            throw new NotImplementedException();
        }


    }
}
