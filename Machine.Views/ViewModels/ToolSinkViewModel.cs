using Machine.ViewModels.Base;
using Machine.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Machine.Views.ViewModels
{
    class ToolSinkViewModel : BaseViewModel
    {
        private Tuple<int, string> _lastSelected;
        public int Id { get; set; }
        public string Name { get; set; }

        private Tuple<int, string> _selectedTool;
        public Tuple<int, string> SelectedTool
        {
            get { return _selectedTool; }
            set
            {
                _lastSelected = _selectedTool;

                if (Set(ref _selectedTool, value, nameof(SelectedTool)))
                {
                    ApplyTooling();
                    (UnloadCommand as RelayCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<Tuple<int, string>> Tools { get; set; } = new ObservableCollection<Tuple<int, string>>();

        private ICommand _unloadCommand;
        public ICommand UnloadCommand => _unloadCommand ?? (_unloadCommand = new RelayCommand(() => SelectedTool = null, () => SelectedTool != null));


        public ToolSinkViewModel() : base()
        {
            Messenger.Register<AutoSourceToolholderChangedMessage>(this, OnAutoSourceToolholderChangedMessage);
        }

        private void OnAutoSourceToolholderChangedMessage(AutoSourceToolholderChangedMessage msg)
        {
            var tuple = Tools.FirstOrDefault(t => t.Item1 == msg.ToolholderId);

            if (tuple != null) Tools.Remove(tuple);
            if ((_selectedTool != null) && (Id == msg.ToolholderId))
            {
                _lastSelected = null;
                SelectedTool = null;
            }
            if (!string.IsNullOrEmpty(msg.ToolName))Tools.Add(new Tuple<int, string>(msg.ToolholderId, msg.ToolName));
        }

        private void ApplyTooling()
        {
            if (_lastSelected != null)
            {
                Messenger.Send(new MoveToolRequestMessage() { Source = Id , Sink = _lastSelected.Item1 });
            }

            if (_selectedTool != null)
            {
                Messenger.Send(new MoveToolRequestMessage() { Source = _selectedTool.Item1, Sink = Id });
            }
        }
    }
}
