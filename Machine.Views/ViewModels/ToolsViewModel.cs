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

namespace Machine.Views.ViewModels
{
    internal class ToolsViewModel : BaseViewModel
    {
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
                }
            }
        }


        public ToolsViewModel()
        {
            Kernel = Machine.ViewModels.Ioc.SimpleIoc<IKernelViewModel>.GetInstance();

            Kernel.Machines.Add(new StaticToolholderElementViewModel()
            {
                Position = new Data.Base.Point(),
                Direction = new Data.Base.Vector() { X = 0.0, Y = 0.0, Z = -1.0 },
            });

            Messenger.Register<LoadToolsMessage>(this, OnLoadToolsMessage);
            Messenger.Register<SaveToolsMessage>(this, OnSaveToolsMessage);
            Messenger.Register<UnloadAllToolMessage>(this, OnUnloadAllToolMessage);
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
    }
}
