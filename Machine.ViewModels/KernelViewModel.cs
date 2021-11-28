using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            set
            {
                //var lastSelcted = _selected;

                if(Set(ref _selected, value, nameof(Selected)))
                {
                    SelectedChanged?.Invoke(this, null);
                }
            }
        }

        public event EventHandler SelectedChanged;

        private ICommand _unloadAllMachineCommand;
        public ICommand UnloadAllMachineCommand => _unloadAllMachineCommand ?? (_unloadAllMachineCommand = new RelayCommand(() => _machines.Clear()));

        //private static void ManageSelectionState(IMachineElement element, bool state)
        //{
        //    if (element is IViewElementData ved) ved.IsSelected = true;
        //}
    }
}
