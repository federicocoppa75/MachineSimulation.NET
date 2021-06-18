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
        private bool _memAutoStepOver;
        private bool _memMultiChannel;
        private int _autoStepOverLimit = -1;
        private int _subGroupIndex;

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
                    OnSelectionChanged();
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
            else if(_autoStepOverLimit > 0)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(50);
                    ManageFarwardSubGroupExecution();
                });
            }
        }

        protected abstract void OnSelectionChanged();

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
            int nSteps = selected.Index - lastSelected.Index;

            if (AutoStepOver && (nSteps > 1))
            {
                ManageFarwardSubgroupStartExecution(lastSelected.Index + 1, selected.Index);
            }
            else
            {
                for (int i = lastSelected.Index + 1; i <= selected.Index; i++)
                {
                    Steps[i].ExecuteFarward();
                }
            }

        }

        private void ManageBackSelectionChanged(StepViewModel selected, StepViewModel lastSelected)
        {
            OnBackSelectionChangeStart();

            for (int i = lastSelected.Index; i > selected.Index; i--)
            {
                //_stepObserver.SetBackIndex(i);
                Steps[i].ExecuteBack();
            }

            OnBackSelectionChangeEnd();
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

        protected virtual void OnBackSelectionChangeStart() 
        {
            _memAutoStepOver = AutoStepOver;
            _memMultiChannel = MultiChannel;
            AutoStepOver = false;
            MultiChannel = false;
        }
        
        protected virtual void OnBackSelectionChangeEnd() 
        {
            AutoStepOver = _memAutoStepOver;
            MultiChannel = _memMultiChannel;
        }

        private void ManageFarwardSubgroupStartExecution(int startIndex, int endIndex)
        {
            if (_autoStepOverLimit == -1)
            {
                AutoStepOver = false;
                _subGroupIndex = startIndex;
                _autoStepOverLimit = endIndex;
                Steps[_subGroupIndex++].ExecuteFarward();
            }
        }

        private void ManageFarwardSubGroupExecution()
        {
            if (_subGroupIndex <= _autoStepOverLimit)
            {
                Steps[_subGroupIndex++].ExecuteFarward();
            }
            else
            {
                _autoStepOverLimit = -1;
                AutoStepOver = true;
            }
        }
    }
}
