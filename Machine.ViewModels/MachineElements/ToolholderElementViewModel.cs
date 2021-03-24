using Machine.Data.Enums;
using Machine.Data.MachineElements;
using Machine.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class ToolholderElementViewModel : ElementViewModel
    {
        private static Color _toolColor = new Color() { A = 255, B = 255 };
        private static Color _coneColor = new Color() { A = 255, B = 128, G = 128, R = 128 };

        public int ToolHolderId { get; set; }
        public ToolHolderType ToolHolderType { get; set; }
        public Vector Position { get; set; }
        public Vector Direction { get; set; }

        public ToolholderElementViewModel() : base()
        {
            Messenger.Register<LoadToolMessage>(this, OnLoadToolMessage);
            Messenger.Register<UnloadToolMessage>(this, OnUnloadToolMessage);
            Messenger.Register<UnloadAllToolMessage>(this, OnUnloadAllToolMessage);
        }

        private void OnLoadToolMessage(LoadToolMessage msg)
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

        private void OnUnloadToolMessage(UnloadToolMessage msg)
        {
            if (msg.ToolHolder == ToolHolderId)
            {
                Children.Clear();
            }
        }

        private void OnUnloadAllToolMessage(UnloadAllToolMessage msg) => Children.Clear();
    }
}
