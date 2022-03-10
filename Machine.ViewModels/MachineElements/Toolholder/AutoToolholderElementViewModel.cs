using Machine.ViewModels.Messages.Tooling;
using Machine.ViewModels.UI;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Machine.ViewModels.MachineElements.Toolholder
{
    public abstract class AutoToolholderElementViewModel : ToolholderElementViewModel
    {
        private IDispatcherHelper _dispatcherHelper;
        private IDispatcherHelper DispatcherHelper => _dispatcherHelper ?? (_dispatcherHelper = Machine.ViewModels.Ioc.SimpleIoc<IDispatcherHelper>.GetInstance());


        public AutoToolholderElementViewModel() : base()
        {
            Messenger.Register<MoveToolRequestMessage>(this, OnMoveToolRequestMessage);
            Messenger.Register<GetToolHolderSinkMessage>(this, OnGetToolHolderSinkMessage);
        }

        private void OnGetToolHolderSinkMessage(GetToolHolderSinkMessage msg)
        {
            if(msg.Sink == ToolHolderId)
            {
                msg?.SetToolHolder(this);
            }
        }

        private void OnMoveToolRequestMessage(MoveToolRequestMessage msg)
        {
            if ((msg.Source == ToolHolderId) && (Children.Count == 1))
            {
                var t = Children.First();

                Messenger.Send(new GetToolHolderSinkMessage()
                {
                    Sink = msg.Sink,
                    SetToolHolder = (th) =>
                    {

                        DispatcherHelper.CheckBeginInvokeOnUi(() =>
                        {                            
                            Children.Remove(t);
                            th.Children.Add(t);
                            t.Parent = th;
                        });
                    }
                });
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

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            Messenger.Unregister<MoveToolRequestMessage>(this);
            Messenger.Unregister<GetToolHolderSinkMessage>(this);
            base.Dispose(disposing);
        }
        #endregion
    }
}
