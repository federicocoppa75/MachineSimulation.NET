using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.Machine.ViewModels
{
    public class MachineElementViewModel : ViewModelBase
    {
        public string Name { get; set; }

        public ObservableCollection<MachineElementViewModel> Children { get; set; } = new ObservableCollection<MachineElementViewModel>();
    }
}
