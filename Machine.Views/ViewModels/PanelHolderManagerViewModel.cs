using Machine.Data.Enums;
using Machine.ViewModels.Base;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.Messages;
using Machine.Views.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Machine.Views.ViewModels
{
    class PanelHolderManagerViewModel : BaseViewModel
    {
        private bool _panelHold = false;
        public bool PanelHold
        {
            get => _panelHold;
            private set
            {
                if (Set(ref _panelHold, value, nameof(PanelHold))) UpdateCanExecuteCommands();
            }
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public PanelLoadType Corner { get; set; }

        private ICommand _loadPanel;
        public ICommand LoadPanel => _loadPanel ?? (_loadPanel = new RelayCommand(LoadPanelImpl, () => !PanelHold));

        private ICommand _unloadPanel;
        public ICommand UnloadPanel => _unloadPanel ?? (_unloadPanel = new RelayCommand(UnloadPanelImpl, () => PanelHold));


        private void LoadPanelImpl()
        {
            PanelData panel = null;

            Messenger.Send(new GetPanelDataMessage() { SetPanelData = (d) => panel = d });

            if (panel != null)
            {
                Messenger.Send(new LoadPanelMessage()
                {
                    PanelHolderId = Id,
                    Length = panel.Length,
                    Width = panel.Width,
                    Height = panel.Height,
                    NotifyExecution = (b) => PanelHold = b
                });

            }
            else
            {
                throw new InvalidOperationException("Panel data must not be null!");
            }
        }

        private void UnloadPanelImpl()
        {
            Messenger.Send(new UnloadPanelMessage()
            {
                PanelHolderId = Id,
                NotifyExecution = (b) => PanelHold = !b
            });
        }

        private void UpdateCanExecuteCommands()
        {
            (_loadPanel as RelayCommand)?.RaiseCanExecuteChanged();
            (_unloadPanel as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
