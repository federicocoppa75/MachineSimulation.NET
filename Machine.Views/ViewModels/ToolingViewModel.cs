using Machine.ViewModels;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Messages.Tooling;
using Machine.ViewModels.UI;
using Machine.Views.Messages.ToolsEditor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MDIT = Machine.Data.Interfaces.Tools;

namespace Machine.Views.ViewModels
{
    internal class ToolingViewModel : BaseElementsCollectionViewModel, IToolingEditor
    {
        private MDIT.ITool _selectedTool;

        public ObservableCollection<ToolingItemViewModel> ToolingItems { get; private set; } = new ObservableCollection<ToolingItemViewModel>();

        private ToolingItemViewModel _selected;

        public ToolingItemViewModel Selected
        {
            get => _selected;
            set
            {
                if(Set(ref _selected, value, nameof(Selected)))
                {
                    UpdateCanExecuteCommands();
                }
            }
        }

        private ICommand _attachToolCommand;
        public ICommand AttachToolCommand => _attachToolCommand ?? (_attachToolCommand = new RelayCommand(() => AttachToolCommandImpl(), () => AttachToolCommandCanExecute()));

        private ICommand _detachToolCommand;
        public ICommand DetachToolCommand => _detachToolCommand ?? (_detachToolCommand = new RelayCommand(() => DetachToolCommandImpl(), () => DetachToolCommandCanExecute()));

        public ToolingViewModel() : base()
        {
            Machine.ViewModels.Ioc.SimpleIoc<IToolingEditor>.Register(this);
            Messenger.Register<ToolSelectionChangedMessage>(this, OnToolSelectionChangedMessage);
            Messenger.Register<SaveToolingMessage>(this, OnSaveToolingMessage);
        }

        protected override void AddElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var element in elements)
            {
                BrowseElements(element, (e) =>
                {
                    if (e is IToolholderElement th)
                    {
                        ToolingItems.Add(new ToolingItemViewModel() { Toolholder = th });
                    }
                });
            }
        }

        protected override void RemoveElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var element in elements)
            {
                BrowseElements(element, (e) =>
                {
                    if (e is IToolholderElement th)
                    {
                        var item = ToolingItems.First(t => object.ReferenceEquals(t.Toolholder, th));
                        ToolingItems.Remove(item);
                    }
                });
            }
        }


        protected override void Clear() => ToolingItems.Clear();

        private void BrowseElements(IMachineElement element, Action<IMachineElement> action)
        {
            action(element);

            if ((element.Children != null) && (element.Children.Count > 0))
            {
                foreach (var child in element.Children)
                {
                    BrowseElements(child, action);
                }
            }
        }

        private bool DetachToolCommandCanExecute()
        {
            if ((_selected != null) && (_selected.Toolholder != null))
            {
                var t = _selected.Toolholder.Children.FirstOrDefault(t => (t is IToolElement) || (t is IAngularTransmission));

                return t != null;
            }
            else
            {
                return false;
            }
        }

        private void DetachToolCommandImpl()
        {
            Messenger.Send(new UnloadToolMessage() { ToolHolder = _selected.Toolholder.ToolHolderId });
            UpdateCanExecuteCommands();
        }

        private bool AttachToolCommandCanExecute()
        {
            if ((_selectedTool != null) && (_selected != null) && (_selected.Toolholder != null))
            {
                var t = _selected.Toolholder.Children.FirstOrDefault(t => (t is IToolElement) || (t is IAngularTransmission));

                return t == null;
            }
            else
            {
                return false;
            }
        }

        private void AttachToolCommandImpl()
        {
            Messenger.Send(new LoadToolMessage()
            {
                ToolHolder = _selected.Toolholder.ToolHolderId,
                Tool = _selectedTool
            });
            UpdateCanExecuteCommands();
        }

        private void OnToolSelectionChangedMessage(ToolSelectionChangedMessage msg)
        {
            _selectedTool = msg.Tool;
            UpdateCanExecuteCommands();
        }

        private void UpdateCanExecuteCommands()
        {
            (_attachToolCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (_detachToolCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void OnSaveToolingMessage(SaveToolingMessage msg)
        {
            foreach (var item in ToolingItems)
            {
                msg.AddToolUnit(item.Toolholder.ToolHolderId, item.ToolName);
            }
        }
    }
}
