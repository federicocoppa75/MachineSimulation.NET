using Machine.ViewModels.Base;
using Machine.ViewModels.UI;
using MachineSteps.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MVMIoc = Machine.ViewModels.Ioc;

namespace Machine.Views.ViewModels
{
    class StepsViewModel : BaseViewModel, IStepsController
    {
        public ObservableCollection<StepViewModel> Steps { get; private set; } = new ObservableCollection<StepViewModel>();

        #region IStepController implementation

        private string _fileOpened;
        public string FileOpened
        {
            get => _fileOpened; 
            set => Set(ref _fileOpened, value, nameof(FileOpened));
        }

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

        #endregion

        #region private methods
        private void LoadStepsCommandImplementation()
        {
            var dlg = MVMIoc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");

            dlg.AddExtension = true;
            dlg.DefaultExt = "msteps";
            dlg.Filter = "Machine step |*.msteps";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineStepsDocument));

                using (var reader = new System.IO.StreamReader(dlg.FileName))
                {
                    var doc = (MachineStepsDocument)serializer.Deserialize(reader);

                    if (doc != null)
                    {
                        Steps.Clear();

                        for (int i = 0; i < doc.Steps.Count; i++)
                        {
                            Steps.Add(new StepViewModel(doc.Steps[i], i + 1));
                        }
                        //ShowMacineStepsDocument(doc);
                        FileOpened = $"MainWindow - {dlg.FileName}";
                    }
                }
            }
        }

        private void UnloadStepsCommandImplementation()
        {
            if (Steps.Count > 0)
            {
                //Selected = Steps[0];
                Steps.Clear();
                //Selected = null;
            }
        }

        private bool PanelPresenceConfirm()
        {
            return false;
        }

        private void ExportPanelCommandImplementation()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
