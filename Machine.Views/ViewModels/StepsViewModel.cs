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
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces;

namespace Machine.Views.ViewModels
{
    partial class StepsViewModel : MSVM.StepsViewModel, IStepsController, IStepsExecutionController
    {
        private bool _memDynamicTransition;

        private IStepsSource _stepsSource;

        private ILinkMovementController _linkMovementController;
        private ILinkMovementController LinkMovementController => _linkMovementController ?? (_linkMovementController = GetInstance<ILinkMovementManager>());

        #region IStepExecutionController implementation
        public bool DynamicTransition
        {
            get => LinkMovementController.Enable;
            set
            {
                if(LinkMovementController.Enable != value)
                {
                    LinkMovementController.Enable = value;
                    RisePropertyChanged(nameof(DynamicTransition));
                    if (!value) AutoStepOver = false;
                }
            }
        }

        private bool _autoStepOver;
        public override bool AutoStepOver
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
        public override bool MultiChannel
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

        private TimeSpanFactor _timeSpanFactor;
        public TimeSpanFactor TimeSpanFactor 
        { 
            get => _timeSpanFactor; 
            set
            {
                if(Set(ref _timeSpanFactor, value, nameof(TimeSpanFactor)))
                {
                    LinkMovementController.TimespanFactor = ToValue(_timeSpanFactor);
                }
            }
        }

        public IEnumerable<TimeSpanFactor> TimeSpanFactors => Enum.GetValues(typeof(TimeSpanFactor)).Cast<TimeSpanFactor>();
        public IEnumerable<SampleTimeOption> SampleTimeOptions => Enum.GetValues(typeof(SampleTimeOption)).Cast<SampleTimeOption>();

        private bool _isStepTimeVisible;
        public bool IsStepTimeVisible 
        { 
            get => _isStepTimeVisible; 
            set => Set(ref _isStepTimeVisible, value, nameof(IsStepTimeVisible)); 
        }

        private TimeSpan _stepTime;
        public TimeSpan StepTime 
        { 
            get => _stepTime; 
            set => Set(ref _stepTime, value, nameof(StepTime)); 
        }

        private SampleTimeOption _minimumSampleTime;
        public SampleTimeOption MinimumSampleTime 
        { 
            get => _minimumSampleTime; 
            set
            {
                if(Set(ref _minimumSampleTime, value, nameof(MinimumSampleTime)))
                {
                    LinkMovementController.MinTimespam = ToValue(_minimumSampleTime);
                }
            }
        }
        #endregion

        #region IStepController implementation
        public string Name => "File.iso";

        public ICommand LoadStepsCommand 
        { 
            get 
            {
                if (_stepsSource == null) _stepsSource = MVMIoc.SimpleIoc<IStepsSource>.GetInstance("File.iso");

                return _stepsSource.LoadStepsCommand;
            } 
        }

        private ICommand _unloadStepsCommand;
        public ICommand UnloadStepsCommand { get { return _unloadStepsCommand ?? (_unloadStepsCommand = new RelayCommand(() => UnloadStepsCommandImplementation())); } }

        #endregion

        public StepsViewModel() : base()
        {
            MVMIoc.SimpleIoc<IOptionProvider<TimeSpanFactor>>.Register(new EnumOptionProxy<TimeSpanFactor>(() => TimeSpanFactors, () => TimeSpanFactor, (v) => TimeSpanFactor = v));
            MVMIoc.SimpleIoc<IOptionProvider<SampleTimeOption>>.Register(new EnumOptionProxy<SampleTimeOption>(() => SampleTimeOptions, () => MinimumSampleTime, (v) => MinimumSampleTime = v));
            MVMIoc.SimpleIoc<IProgressState>.Register(this);
            (Steps as ObservableCollection<MSVM.StepViewModel>).CollectionChanged += OnStepsChanged;
        }

        protected override void OnSelectionChanged()
        {
            StepTime = TimeSpan.FromSeconds((Selected != null) ? Selected.EvolutionTime : 0.0);
        }

        private void OnStepsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(Steps.Count > 0)
            {
                UpdateEvolutionTime();
                IsStepTimeVisible = true;
            }
            else
            {
                IsStepTimeVisible = false;
            }
        }

        private void UnloadStepsCommandImplementation()
        {
            if (Steps.Count > 0)
            {
                Selected = Steps[0];
                Steps.Clear();
                Selected = null;
                SourceName = string.Empty;
                ProgressIndex = -1;
            }
        }

        protected override void OnBackSelectionChangeStart()
        {
            base.OnBackSelectionChangeStart();
            _memDynamicTransition = DynamicTransition;
            DynamicTransition = false;
        }

        protected override void OnBackSelectionChangeEnd()
        {
            base.OnBackSelectionChangeEnd();
            DynamicTransition = _memDynamicTransition;
        }

        private double ToValue(TimeSpanFactor timeSpanFactor)
        {
            var result = 1.0;

            switch (timeSpanFactor)
            {
                case TimeSpanFactor.Factor1:
                    result = 1.0;
                    break;
                case TimeSpanFactor.Factor2:
                    result = 2.0;
                    break;
                case TimeSpanFactor.Factor5:
                    result = 5.0;
                    break;
                case TimeSpanFactor.Factor10:
                    result = 10.0;
                    break;
                default:
                    break;
            }

            return result;
        }

        private int ToValue(SampleTimeOption option)
        {
            var result = 20;

            switch (option)
            {
                case SampleTimeOption.Sample_20ms:
                    result = 20;
                    break;
                case SampleTimeOption.Sample_30ms:
                    result = 30;
                    break;
                case SampleTimeOption.Sample_40ms:
                    result = 40;
                    break;
                case SampleTimeOption.Sample_50ms:
                    result = 50;
                    break;
                case SampleTimeOption.Sample_min:
                    result = 0;
                    break;
                default:
                    break;
            }

            return result;
        }

        private void UpdateEvolutionTime()
        {
            double time = 0.0;

            for (int i = 0; i < Steps.Count; i++)
            {
                time += Steps[i].Duration;
                Steps[i].EvolutionTime = time;
            }
        }
    }
}
