using Machine.Data.Interfaces.Tools;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.MachineElements.Toolholder;
using Machine.ViewModels.Messages.Tooling;
using Machine.ViewModels.UI;
using Machine.Views.ViewModels.ToolProxies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Machine.Views.ViewModels
{
    internal class ToolsViewModel : BaseViewModel, IToolsetEditor
    {
        class AddToolCommand : IAddToolCommand
        {
            public string Label { get; set; }
            public ICommand Command { get; set; }
        }

        public IKernelViewModel Kernel { get; private set; }

        public ObservableCollection<ToolProxyViewModel> Tools { get; private set; } = new ObservableCollection<ToolProxyViewModel>();

        private ToolProxyViewModel _selected;

        public ToolProxyViewModel Selected
        {
            get => _selected; 
            set 
            {
                var last = _selected;

                if(Set(ref _selected, value, nameof(Selected)))
                {
                    if (last != null) Messenger.Send(new UnloadToolMessage());
                    if (_selected != null) Messenger.Send(new LoadToolMessage() { Tool = _selected.GetTool() });
                    (_deleteCommand as RelayCommand).RaiseCanExecuteChanged();
                }
            }
        }

        #region IToolsetEditor
        private IEnumerable<IAddToolCommand> _addCommands;
        public IEnumerable<IAddToolCommand> AddCommands => _addCommands ?? (_addCommands = CreateAddCommands());

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(() => DeleteCommandImpl(), () => DeleteCommandCanExecute()));
        #endregion

        public ToolsViewModel()
        {
            Kernel = GetInstance<IKernelViewModel>();

            Kernel.Machines.Add(new StaticToolholderElementViewModel()
            {
                Position = new Data.Base.Point(),
                Direction = new Data.Base.Vector() { X = 0.0, Y = 0.0, Z = -1.0 },
            });

            Messenger.Register<LoadToolsMessage>(this, OnLoadToolsMessage);
            Messenger.Register<SaveToolsMessage>(this, OnSaveToolsMessage);
            Messenger.Register<UnloadAllToolMessage>(this, OnUnloadAllToolMessage);

            Machine.ViewModels.Ioc.SimpleIoc<IToolsetEditor>.Register(this);
        }

        private void OnSaveToolsMessage(SaveToolsMessage msg)
        {
            var tools = Tools.Select(t => t.GetTool());

            msg.GetTools?.Invoke(tools);
        }

        private void OnLoadToolsMessage(LoadToolsMessage msg)
        {
            Tools.Clear();

            foreach (var item in msg.Tools)
            {
                Tools.Add(item.Convert());
            }

            AdjustView();
        }

        private void OnUnloadAllToolMessage(UnloadAllToolMessage msg) => Tools.Clear();

        private void AdjustView()
        {
            if (HasInstance<ICameraControl>())
            {
                var cameraControl = GetInstance<ICameraControl>();

                cameraControl.SetLookDirection(-190.0, -190.0, -90.0);
                cameraControl.SetUpDirection(0.0, 0.0, 1.0);
                cameraControl.SetPosition(190.0, 190.0, 40.0);
            }
        }

        #region IToolsEditor implementation
        private bool DeleteCommandCanExecute() => _selected != null;

        private void DeleteCommandImpl()
        {
            Tools.Remove(_selected);
            Selected = null;
        }

        private IEnumerable<IAddToolCommand> CreateAddCommands()
        {
            var list = new List<IAddToolCommand>();

            list.Add(new AddToolCommand() { Label = "Simple", Command = new RelayCommand(() => AddTool(new SimpleToolProxyViewModel()))});
            list.Add(new AddToolCommand() { Label = "Pointed", Command = new RelayCommand(() => AddTool(new PointedToolProxyViewModel()))});
            list.Add(new AddToolCommand() { Label = "Two section", Command = new RelayCommand(() => AddTool(new TwoSectionToolProxyViewModel()))});
            list.Add(new AddToolCommand() { Label = "Countersink", Command = new RelayCommand(() => AddTool(new CountersinkProxyToolViewModel()))});
            list.Add(new AddToolCommand() { Label = "Disk", Command = new RelayCommand(() => AddTool(new DiskToolProxyViewModel()))});
            list.Add(new AddToolCommand() { Label = "Disk on cone", Command = new RelayCommand(() => AddTool(new DiskOnConeToolProxyViewModel()))});

            return list;
        }

        private void AddTool(ToolProxyViewModel tool)
        {
            Tools.Add(tool);
            Selected = tool;
            AdjustView();
        }
        #endregion
    }
}
