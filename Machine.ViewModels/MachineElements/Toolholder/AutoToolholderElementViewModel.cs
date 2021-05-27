using Machine.ViewModels.Messages.Tooling;
using System;
using System.Linq;

namespace Machine.ViewModels.MachineElements.Toolholder
{
    public abstract class AutoToolholderElementViewModel : ToolholderElementViewModel
    {
        public AutoToolholderElementViewModel() : base()
        {
            Messenger.Register<MoveToolRequestMessage>(this, OnMoveToolRequestMessage);
            Messenger.Register<MoveToolExecuteMessage>(this, OnMoveToolExecuteMessage);
        }

        private void OnMoveToolExecuteMessage(MoveToolExecuteMessage msg)
        {
            if ((msg.Sink == ToolHolderId) && (Children.Count == 0))
            {
                Children.Add(msg.Tool);
            }
        }

        private void OnMoveToolRequestMessage(MoveToolRequestMessage msg)
        {
            if ((msg.Source == ToolHolderId) && (Children.Count == 1))
            {
                var t = Children.First();

                Children.Remove(t);
                Messenger.Send(new MoveToolExecuteMessage() { Sink = msg.Sink, Tool = t });
            }
        }

        protected override void OnUnloadToolMessage(UnloadToolMessage msg) => NotifyChildrenChangedAfterAction(() => base.OnUnloadToolMessage(msg));

        protected override void OnUnloadAllToolMessage(UnloadAllToolMessage msg) => NotifyChildrenChangedAfterAction(() => base.OnUnloadAllToolMessage(msg));

        protected void NotifyChildrenChangedAfterAction(Action action) => NotifyChildrenChangedAfterAction(action, string.Empty);

        protected void NotifyChildrenChangedAfterAction(Action action, string toolName)
        {
            var n = Children.Count;

            action();

            if (n != Children.Count) Messenger.Send(new AutoSourceToolholderChangedMessage { ToolholderId = ToolHolderId, ToolName = toolName });
        }
    }
}
