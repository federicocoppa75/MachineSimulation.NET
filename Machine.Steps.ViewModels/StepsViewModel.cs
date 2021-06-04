using Machine.Steps.ViewModels.Interfaces;
using Machine.Steps.ViewModels.Messages;
using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Steps.ViewModels
{
    public abstract class StepsViewModel : BaseViewModel, IStepsContainer
    {
        public string SourceName { get; set; }
        public IList<StepViewModel> Steps { get; private set; } = new ObservableCollection<StepViewModel>();

        private StepViewModel _selected;
        public StepViewModel Selected
        {
            get => _selected;
            set
            {
                var lastSelected = _selected;

                if(Set(ref _selected, value, nameof(Selected)))
                {
                    ManageSelectionChanged(_selected, lastSelected);
                }
            }
        }

        public abstract bool AutoStepOver { get; set; }
        public abstract bool MultiChannel { get; set; }

        public StepsViewModel() : base()
        {
            Messenger.Register<StepCompletedMessage>(this, OnStepCompletedMessage);
        }

        private void OnStepCompletedMessage(StepCompletedMessage msg)
        {
            if(AutoStepOver)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(50);
                    StepViewModel newSelection = GetNextStep();

                    //if (newSelection != null) DispatcherHelperEx.CheckBeginInvokeOnUI(() => Selected = newSelection);
                    if (newSelection != null) Selected = newSelection;
                });
            }
        }

        private void ManageSelectionChanged(StepViewModel selected, StepViewModel lastSelected)
        {
            if (lastSelected == null)
            {
                ManageFarwardSelectionChanged(selected, Steps[0]);
            }
            else if (selected == null)
            {
                // per il momento non faccio nulla
            }
            else if (selected.Index > lastSelected.Index)
            {
                ManageFarwardSelectionChanged(selected, lastSelected);
            }
            else if (selected.Index < lastSelected.Index)
            {
                ManageBackSelectionChanged(selected, lastSelected);
            }
        }

        private void ManageFarwardSelectionChanged(StepViewModel selected, StepViewModel lastSelected)
        {
            for (int i = lastSelected.Index + 1; i <= selected.Index; i++)
            {
                Steps[i].ExecuteFarward();
            }
        }

        private void ManageBackSelectionChanged(StepViewModel selected, StepViewModel lastSelected)
        {
            for (int i = lastSelected.Index; i > selected.Index; i--)
            {
                //_stepObserver.SetBackIndex(i);
                Steps[i].ExecuteBack();
            }
        }

        private StepViewModel GetNextStep()
        {
            StepViewModel newSelection = null;

            if (_selected == null)
            {
                newSelection = Steps[0];
            }
            else
            {
                int index = Steps.IndexOf(Selected) + 1;

                if (index < Steps.Count())
                {
                    newSelection = Steps[index];
                }
            }

            return newSelection;
        }
    }
}
