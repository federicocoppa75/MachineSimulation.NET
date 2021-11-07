using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Machine.ViewModels.UI
{


    public interface IProbesController
    {
        bool Active { get; set; }
        ICommand DistanceCommand { get; }
        ICommand RemoveCommand { get; }
        ICommand RemoveAllCommand { get; }
    }
}
