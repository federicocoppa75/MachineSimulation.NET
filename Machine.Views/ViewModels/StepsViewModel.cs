using Machine.ViewModels.Base;
using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MVMIoc = Machine.ViewModels.Ioc;
using MSVM = Machine.Steps.ViewModels;

namespace Machine.Views.ViewModels
{
    partial class StepsViewModel : MSVM.StepsViewModel, IStepsController, IStepsExecutionController
    {
        private IStepsSource _stepsSource;

        #region IStepExecutionController implementation
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
        #endregion

        #region IStepController implementation
        public string Name => "File.msteps";

        public ICommand LoadStepsCommand 
        { 
            get 
            {
                if (_stepsSource == null) _stepsSource = MVMIoc.SimpleIoc<IStepsSource>.GetInstance();

                return _stepsSource.LoadStepsCommand;
            } 
        }

        private ICommand _unloadStepsCommand;
        public ICommand UnloadStepsCommand { get { return _unloadStepsCommand ?? (_unloadStepsCommand = new RelayCommand(() => UnloadStepsCommandImplementation())); } }

        #endregion

        private void UnloadStepsCommandImplementation()
        {
            if (Steps.Count > 0)
            {
                Selected = Steps[0];
                Steps.Clear();
                Selected = null;
            }
        }
    }
}
