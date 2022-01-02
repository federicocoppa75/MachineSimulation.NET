using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Tools;
using Machine.ViewModels.MachineElements.Toolholder;
using Machine.ViewModels.Messages.Tooling;
using Machine.ViewModels.Tools;
using Machine.ViewModels.UI;
using Machine.Views.Messages.ToolsEditor;
using Machine.Views.ViewModels.ToolProxies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Machine.Views.ViewModels
{
    internal class ToolsViewModel : BaseViewModel, IToolsetEditor, IDataUnloader
    {
        class AddToolCommand : IAddToolCommand
        {
            public string Label { get; set; }
            public ICommand Command { get; set; }
        }

        private ToolDimensionViewModel _toolDimension;

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
                    if (last != null) last.Unload();
                    if (_selected != null) _selected.Load();
                    (_deleteCommand as RelayCommand).RaiseCanExecuteChanged();
                    (_addCopyCommand as RelayCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private object _selectedProperty;
        public object SelectedProperty 
        { 
            get => _selectedProperty; 
            set
            {
                if(Set(ref _selectedProperty, value, nameof(SelectedProperty)))
                {
                    ManagePropertyDimension();
                }
            } 
        }

        #region IToolsetEditor
        private IEnumerable<IAddToolCommand> _addCommands;
        public IEnumerable<IAddToolCommand> AddCommands => _addCommands ?? (_addCommands = CreateAddCommands());

        private ICommand _addCopyCommand;
        public ICommand AddCopyCommand => _addCopyCommand ?? (_addCopyCommand = new RelayCommand(() => AddCopyCommandImpl(), () => AddCopyCommandCanExecute()));

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(() => DeleteCommandImpl(), () => DeleteCommandCanExecute()));
        #endregion

        #region IDataUnloader
        private ICommand _unloadCommand;
        public ICommand UnloadCommand => _unloadCommand ?? (_unloadCommand = new RelayCommand(() => UnloadCommandImpl(), () => UnloadCommandCanExecute()));
        #endregion

        private ICommand _propertyValueChanged;
        public ICommand PropertyValueChanged => _propertyValueChanged ?? (_propertyValueChanged = new RelayCommand(() => ManagePropertyDimension()));

        public ToolsViewModel()
        {
            _toolDimension = new ToolDimensionViewModel() { Name = "Tool dimension" };
            Kernel = GetInstance<IKernelViewModel>();

            Kernel.Machines.Add(new StaticToolholderElementViewModel()
            {
                Position = new Data.Base.Point(),
                Direction = new Data.Base.Vector() { X = 0.0, Y = 0.0, Z = -1.0 },
            });

            Kernel.Machines.Add(_toolDimension);

            Messenger.Register<LoadToolsMessage>(this, OnLoadToolsMessage);
            Messenger.Register<SaveToolsMessage>(this, OnSaveToolsMessage);
            Messenger.Register<UnloadAllToolMessage>(this, OnUnloadAllToolMessage);
            Messenger.Register<ToolsRequestMessage>(this, OnToolsRequestMessage);

            Machine.ViewModels.Ioc.SimpleIoc<IToolsetEditor>.Register(this);
            Machine.ViewModels.Ioc.SimpleIoc<IDataUnloader>.Register(this);
        }

        private void OnToolsRequestMessage(ToolsRequestMessage msg)
        {
            var tools = Tools.Select(t => t.GetTool());

            msg.SetTools(tools);
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
            UpdateUnloadCommandCanExecute();
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
            Messenger.Send(new ToolDeletedMessage() { ToolName = _selected.Name });
            Tools.Remove(_selected);
            Selected = null;
            UpdateUnloadCommandCanExecute();
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
            list.Add(new AddToolCommand() { Label = "Angolar transmission (1 spindle)", Command = new RelayCommand(() => AddTool(new AngolarTransmission1ProxyViewModel()))});
            list.Add(new AddToolCommand() { Label = "Angolar transmission (2 spindle)", Command = new RelayCommand(() => AddTool(new AngolarTransmission2ProxyViewModel()))});
            list.Add(new AddToolCommand() { Label = "Angolar transmission (3 spindle)", Command = new RelayCommand(() => AddTool(new AngolarTransmission3ProxyViewModel()))});

            return list;
        }

        private void AddTool(ToolProxyViewModel tool)
        {
            Tools.Add(tool);
            Selected = tool;
            AdjustView();
            UpdateUnloadCommandCanExecute();
        }

        private bool AddCopyCommandCanExecute() => _selected != null;

        private void AddCopyCommandImpl()
        {
            var tool = _selected.CreateCopy();

            Tools.Add(tool);
            Selected = tool;
        }
        #endregion

        #region IDataUnloader
        private bool UnloadCommandCanExecute() => Tools.Count > 0;

        private void UnloadCommandImpl()
        {
            Selected = null;
            Tools.Clear();
            UpdateUnloadCommandCanExecute();
        }

        private void UpdateUnloadCommandCanExecute() => (_unloadCommand as RelayCommand).RaiseCanExecuteChanged();
        #endregion

        private void ManagePropertyDimension()
        {
            if ((_selectedProperty != null) && (_selectedProperty is string str) && (_selected is IMeasurableTool mt))
            {
                _toolDimension.IsVisible = mt.ProcessDimension(str, _toolDimension);

            }
            else
            {
                _toolDimension.IsVisible = false;
            }
        }
    }
}
