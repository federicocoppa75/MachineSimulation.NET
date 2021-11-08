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
            set
            {
                if(Set(ref _active, value, nameof(Active)))
                {
                    UpdateCommands();
                    UpdateCommandsFromSelection();
                }
            }
        }

        private ICommand _distanceCommand;
        public ICommand DistanceCommand => _distanceCommand ?? (_distanceCommand = new RelayCommand(() => DistanceCommandImpl(), () => Active && CanExecuteAddDistanceProbe()));

 
        private ICommand _removeCommand;
        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new RelayCommand(() => RemoveCommandImpl(), () => (Probes.Count > 0) && Probes.Any(p => (p is IViewElementData ved) && ved.IsSelected)));

        private ICommand _removeAllCommand;
        public ICommand RemoveAllCommand => _removeAllCommand ?? (_removeAllCommand = new RelayCommand(() => RemoveAllCommandImpl(), () => Probes.Count > 0));
        #endregion

        public ProbesViewModel() : base()
        {
            Machine.ViewModels.Ioc.SimpleIoc<IProbesController>.Register(this);
            Messenger.Register<AddProbeMessage>(this, OnAddProbeMessage);
            Messenger.Register<ProbeSelectedChangedMessage>(this, OnProbeSelectedChangedMessage);
        }

        private void OnProbeSelectedChangedMessage(ProbeSelectedChangedMessage msg) => UpdateCommandsFromSelection();


        private void OnAddProbeMessage(AddProbeMessage msg)
        {
            Probes.Add(msg.Probe);
            UpdateCommands();
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

        protected override void Clear() => RemoveAll();

        private void RemoveCommandImpl()
        {
            var selected = Probes.Where(p => (p is IViewElementData ved) && ved.IsSelected).ToList();

            foreach (var item in selected) Remove(item);

            UpdateCommands();
        }

        private void RemoveAllCommandImpl() => RemoveAll();

        private void DistanceCommandImpl()
        {
            var selected = Probes.Where((p) => (p is IViewElementData ved) && ved.IsSelected).ToList();

            if ((selected.Count() == 2) && (selected.All((p) => (p is IProbePoint))))
            {
                var master = selected[0] as IProbePoint;
                var slave = selected[1] as IProbePoint;

                var probe = GetInstance<IProbeFactory>().Create(master, slave);

                Probes.Add(probe);
            }
        }

        private void Remove(IProbe probe, bool removeFromProbes = true)
        {
            if(probe is IMachineElement me)
            {
                me.Parent?.Children.Remove(me);

                foreach (var item in me.Children)
                {
                    if (item is IProbe p) Remove(p); 
                }

                me.Children.Clear();
            }

            Probes.Remove(probe);

            (probe as IDetachableProbe)?.Detach();
        }

        private void RemoveAll()
        {
            foreach (var item in Probes) Remove(item, false);

            Probes.Clear();

            UpdateCommands();
        }


        private void UpdateCommands()
        {
            (RemoveCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RemoveAllCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void UpdateCommandsFromSelection()
        {
            (DistanceCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RemoveCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private bool CanExecuteAddDistanceProbe()
        {
            bool result = false;
            var selected = Probes.Where((p) => (p is IViewElementData ved) && ved.IsSelected);

            if((selected.Count() == 2) && (selected.All((p) => (p is IProbePoint))))
            {
                result = true;
            }

            return result;
        }

        private void RemoveProbeChildren(IProbe probe)
        {
            if ((probe is IMachineElement me) && (me.Children.Count > 0))
            {
                foreach (var item in me.Children)
                {
                    if (item is IProbeDistance pd) RemoveProbeDistance(pd);
                }
            }
        }

        private void RemoveProbeDistance(IProbeDistance probeDistance)
        {
            probeDistance.Master = null;
            probeDistance.Slave = null;
            (probeDistance as IMachineElement).Parent = null;
        }
    }
}
