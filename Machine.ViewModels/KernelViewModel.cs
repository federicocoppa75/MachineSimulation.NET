using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.MachineElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Machine.ViewModels
{
    public class KernelViewModel : BaseViewModel, IKernelViewModel
    {
        private ObservableCollection<IMachineElement> _machines = new ObservableCollection<IMachineElement>();
        public IList<IMachineElement> Machines => _machines;

        private IMachineElement _selected;
        public IMachineElement Selected 
        {
            get => _selected;
            set => Set(ref _selected, value, nameof(Selected));
        }

        private ICommand _unloadAllMachineCommand;
        public ICommand UnloadAllMachineCommand => _unloadAllMachineCommand ?? (_unloadAllMachineCommand = new RelayCommand(() => _machines.Clear()));
    }
}
