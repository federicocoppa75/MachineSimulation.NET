using Machine.ViewModels.Base;
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
        private ObservableCollection<ElementViewModel> _machines = new ObservableCollection<ElementViewModel>();
        public IList<ElementViewModel> Machines => _machines;

        private ElementViewModel _selected;
        public ElementViewModel Selected 
        {
            get => _selected;
            set => Set(ref _selected, value, nameof(Selected));
        }

        private ICommand _unloadAllMachineCommand;
        public ICommand UnloadAllMachineCommand => _unloadAllMachineCommand ?? (_unloadAllMachineCommand = new RelayCommand(() => _machines.Clear()));
    }
}
