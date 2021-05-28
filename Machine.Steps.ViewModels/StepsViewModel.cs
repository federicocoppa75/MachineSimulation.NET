using Machine.Steps.ViewModels.Interfaces;
using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Machine.Steps.ViewModels
{
    public class StepsViewModel : BaseViewModel, IStepsContainer
    {
        public string SourceName { get; set; }
        public IList<StepViewModel> Steps { get; private set; } = new ObservableCollection<StepViewModel>();

        private StepViewModel _selected;
        public StepViewModel Selected
        {
            get => _selected; 
            set => Set(ref _selected, value, nameof(Selected));
        }
    }
}
