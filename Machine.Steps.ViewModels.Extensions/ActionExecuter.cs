using Machine.Steps.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages;
using Machine.ViewModels.Messages.Links;
using Machine.ViewModels.Messages.Links.Gantry;
using Machine.ViewModels.Messages.Tooling;
using Machine.ViewModels.UI;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions
{
    public class ActionExecuter : IActionExecuter
    {
        IMessenger _messenger;
        private IMessenger Messenger => _messenger ?? (_messenger = Machine.ViewModels.Ioc.SimpleIoc<IMessenger>.GetInstance());

        IDispatcherHelper _dispatcherHelper;
        private IDispatcherHelper DispatcherHelper => _dispatcherHelper ?? (_dispatcherHelper = Machine.ViewModels.Ioc.SimpleIoc<IDispatcherHelper>.GetInstance());

        public void Execute(BaseAction action, int notifyId)
        {
            if (action is AddPanelAction apa) Execute(apa, notifyId);
            else if (action is RemovePanelAction rpa) Execute(rpa, notifyId);
            else if (action is LinearPositionLinkAction lpla) Execute(lpla, notifyId);
            else if (action is TwoPositionLinkAction tpla) Execute(tpla, notifyId);
            else if (action is LoadToolAction lta) Execute(lta, notifyId);
            else if (action is UnloadToolAction uta) Execute(uta, notifyId);
            else if (action is LinearPositionLinkGantryOnAction lplgona) Execute(lplgona, notifyId);
            else if (action is LinearPositionLinkGantryOffAction lplgoffa) Execute(lplgoffa, notifyId);
            else if (action is LinearInterpolatedPositionLinkAction lipla) Execute(lipla, notifyId);
            else if (action is ArcInterpolatedPositionLinkAction aipla) Execute(aipla, notifyId);
            else if (action is InjectAction ia) Execute(ia, notifyId);
            else if (action is TurnOffInverterAction toffia) Execute(toffia, notifyId);
            else if (action is TurnOnInverterAction tonia) Execute(tonia, notifyId);
            else if (action is UpdateRotationSpeedAction ursa) Execute(ursa, notifyId);
            else if (action is ChannelWaiterAction cwa) Execute(cwa, notifyId);
            else if (action is NotOperationAction noa) Execute(noa, notifyId);
        }

        public void Execute(AddPanelAction action, int notifyId)
        {
            DispatcherHelper.CheckBeginInvokeOnUi(() =>
            {
                Messenger.Send(new LoadPanelMessage()
                {
                    PanelHolderId = action.PanelHolder,
                    Length = action.XDimension,
                    Width = action.YDimension,
                    Height = action.ZDimension
                });
            });

            NotifyExecuted(notifyId);
        }

        public void Execute(RemovePanelAction action, int notifyId)
        {
            Messenger.Send(new UnloadPanelMessage() { PanelHolderId = action.PanelHolder });
            NotifyExecuted(notifyId);
        }

        public void Execute(LinearPositionLinkAction action, int notifyId)
        {
            Messenger.Send(new GetLinkMessage()
            {
                Id = action.LinkId,
                SetLink = (link) =>
                {
                    link.Value = action.RequestedPosition;                    
                }
            });

            NotifyExecuted(notifyId);
        }

        public void Execute(TwoPositionLinkAction action, int notifyId)
        {
            Messenger.Send(new GetLinkMessage()
            {
                Id = action.LinkId,
                SetLink = (link) =>
                {
                    (link as IPneumaticLinkViewModel).State = action.RequestedState == MachineSteps.Models.Enums.TwoPositionLinkActionRequestedState.On;
                }
            });

            NotifyExecuted(notifyId);
        }

        public void Execute(LoadToolAction action, int notifyId)
        {
            Messenger.Send(new MoveToolRequestMessage()
            {
                Source = action.ToolSource,
                Sink = action.ToolSink
            });

            NotifyExecuted(notifyId);
        }

        public void Execute(UnloadToolAction action, int notifyId)
        {
            Messenger.Send(new MoveToolRequestMessage()
            {
                Source = action.ToolSource,
                Sink = action.ToolSink
            });

            NotifyExecuted(notifyId);
        }

        public void Execute(LinearPositionLinkGantryOnAction action, int notifyId)
        {
            Messenger.Send(new GantryMessage()
            {
                Master = action.MasterId,
                Slave = action.SlaveId,
                State = true
            });

            NotifyExecuted(notifyId);
        }

        public void Execute(LinearPositionLinkGantryOffAction action, int notifyId)
        {
            Messenger.Send(new GantryMessage()
            {
                Master = action.MasterId,
                Slave = action.SlaveId,
                State = false
            });

            NotifyExecuted(notifyId);
        }

        public void Execute(LinearInterpolatedPositionLinkAction action, int notifyId)
        {
            foreach (var item in action.Positions)
            {
                Messenger.Send(new GetLinkMessage()
                {
                    Id = item.LinkId,
                    SetLink = (link) =>
                    {
                        link.Value = item.RequestPosition;
                    }
                });
            }

            NotifyExecuted(notifyId);
        }

        public void Execute(ArcInterpolatedPositionLinkAction action, int notifyId)
        {
            foreach (var item in action.Components)
            {
                Messenger.Send(new GetLinkMessage()
                {
                    Id = item.LinkId,
                    SetLink = (link) =>
                    {
                        link.Value = item.TargetCoordinate;
                    }
                });
            }

            NotifyExecuted(notifyId);
        }

        public void Execute(InjectAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        public void Execute(TurnOffInverterAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        public void Execute(TurnOnInverterAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        public void Execute(UpdateRotationSpeedAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        public void Execute(ChannelWaiterAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        public void Execute(NotOperationAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        private void NotifyExecuted(int notifyId)
        {
            if (notifyId > 0) Messenger.Send(new ActionExecutedMessage() { Id = notifyId });
        }
    }
}
