using Machine.ViewModels.Base;
using Machine.ViewModels.UI;
using System;
using System.Windows.Input;

namespace Machine.Views.UI
{
    public class StepsControllerViewModel : BaseViewModel, IStepsController
    {
        private bool _dynamicTransition;
        public bool DynamicTransition
        {
            get => _dynamicTransition;
            set
            {
                if (Set(ref _dynamicTransition, value, nameof(DynamicTransition)))
                {
                    //NotifyDynamicTransitionChanged();
                    if (!_dynamicTransition) AutoStepOver = false;
                }
            }
        }

        private bool _autoStepOver;
        public bool AutoStepOver
        {
            get => _autoStepOver;
            set
            {
                if (Set(ref _autoStepOver, value, nameof(AutoStepOver)))
                {
                    if (_autoStepOver) DynamicTransition = true;
                    else MultiChannel = false;
                    //MessengerInstance.Send(new AutoStepOverChangedMessage() { Value = _autoStepOver });
                }
            }
        }

        private bool _multiChannel;
        public bool MultiChannel
        {
            get => _multiChannel;
            set
            {
                if (Set(ref _multiChannel, value, nameof(MultiChannel)))
                {
                    //MessengerInstance.Send(new MultiChannelMessage() { Value = _multiChannel });
                }
            }
        }

        private bool _materialRemoval;
        public bool MaterialRemoval
        {
            get => _materialRemoval;
            set
            {
                if (Set(ref _materialRemoval, value, nameof(MaterialRemoval)))
                {
                    //MessengerInstance.Send(new MaterialRemovalMessage() { Active = _materialRemoval });
                }
            }
        }

        private ICommand _loadStepsCommand;
        public ICommand LoadStepsCommand { get { return _loadStepsCommand ?? (_loadStepsCommand = new RelayCommand(() => LoadStepsCommandImplementation())); } }

        private ICommand _unloadStepsCommand;
        public ICommand UnloadStepsCommand { get { return _unloadStepsCommand ?? (_unloadStepsCommand = new RelayCommand(() => UnloadStepsCommandImplementation())); } }

        private ICommand _exportPanelCommand;
        public ICommand ExportPanelCommand { get { return _exportPanelCommand ?? (_exportPanelCommand = new RelayCommand(() => ExportPanelCommandImplementation(), () => PanelPresenceConfirm())); } }

        private void LoadStepsCommandImplementation()
        {
            throw new NotImplementedException();
        }

        private void UnloadStepsCommandImplementation()
        {
            throw new NotImplementedException();
        }

        private bool PanelPresenceConfirm()
        {
            return false;
        }

        private void ExportPanelCommandImplementation()
        {
            throw new NotImplementedException();
        }
    }
}
