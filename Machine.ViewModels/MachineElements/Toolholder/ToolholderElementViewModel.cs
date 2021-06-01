using Machine.Data.Enums;
using Machine.Data.MachineElements;
using Machine.Data.Tools;
using Machine.ViewModels.Messages.Tooling;
using System;

namespace Machine.ViewModels.MachineElements.Toolholder
{
    public abstract class ToolholderElementViewModel : ElementViewModel
    {
        private static Color _toolColor = new Color() { A = 255, B = 255 };
        private static Color _coneColor = new Color() { A = 255, B = 128, G = 128, R = 128 };

        public int ToolHolderId { get; set; }
        public abstract ToolHolderType ToolHolderType { get; }
        public Vector Position { get; set; }
        public Vector Direction { get; set; }

        public ToolholderElementViewModel() : base()
        {
            Messenger.Register<LoadToolMessage>(this, OnLoadToolMessage);
            Messenger.Register<UnloadToolMessage>(this, OnUnloadToolMessage);
            Messenger.Register<UnloadAllToolMessage>(this, OnUnloadAllToolMessage);
            Messenger.Register<AngularTransmissionLoadMessage>(this, OnAngularTransmissionLoadMessage);
        }

        protected virtual void OnAngularTransmissionLoadMessage(AngularTransmissionLoadMessage msg)
        {
            if (msg.ToolHolder == ToolHolderId)
            {
                var vm = new AngularTransmissionViewModel()
                {
                    Name = msg.AngularTransmission.Name,
                    Tool = msg.AngularTransmission,
                    IsVisible = true
                };

                msg.AppendSubSpindle((p, v, t) => vm.AppendSubSpindle(p, v, t));

                Children.Add(vm);
            }
        }

        protected virtual void OnLoadToolMessage(LoadToolMessage msg)
        {
            if (msg.ToolHolder == ToolHolderId)
            {               
                var vm = new ToolViewModel()
                {
                    Name = msg.Tool.Name,
                    Tool = msg.Tool,
                    Color = _toolColor,
                    ConeColor = _coneColor,
                    IsVisible = true
                };

                Children.Add(vm);               
            }
        }

        protected virtual void OnUnloadToolMessage(UnloadToolMessage msg)
        {
            if (msg.ToolHolder == ToolHolderId)
            {
                Children.Clear();
            }
        }

        protected virtual void OnUnloadAllToolMessage(UnloadAllToolMessage msg) => Children.Clear();
    }
}
