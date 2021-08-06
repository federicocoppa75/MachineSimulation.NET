using Machine.Steps.ViewModels.Interfaces;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages;
using Machine.ViewModels.Messages.Links;
using Machine.ViewModels.Messages.Links.Gantry;
using Machine.ViewModels.Messages.Tooling;
using Machine.ViewModels.Messaging;
using MachineSteps.Models.Actions;
using System;
using System.Threading.Tasks;
using static Machine.ViewModels.Interfaces.Links.ILinkMovementManager;

namespace Machine.Steps.ViewModels.Extensions
{
    public class ActionExecuter : IActionExecuter
    {
        static int _interpolationGroupId = 0;

        IMessenger _messenger;
        private IMessenger Messenger => _messenger ?? (_messenger = Machine.ViewModels.Ioc.SimpleIoc<IMessenger>.GetInstance());

        //IDispatcherHelper _dispatcherHelper;
        //private IDispatcherHelper DispatcherHelper => _dispatcherHelper ?? (_dispatcherHelper = Machine.ViewModels.Ioc.SimpleIoc<IDispatcherHelper>.GetInstance());

        ILinkMovementManager _linkMovementManager;
        private ILinkMovementManager LinkMovementManager => _linkMovementManager ?? (_linkMovementManager = Machine.ViewModels.Ioc.SimpleIoc<ILinkMovementManager>.GetInstance());

        private bool IsDynamic => LinkMovementManager.Enable;

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

        private void Execute(AddPanelAction action, int notifyId)
        {
            Messenger.Send(new LoadPanelMessage()
            {
                PanelHolderId = action.PanelHolder,
                Length = action.XDimension,
                Width = action.YDimension,
                Height = action.ZDimension
            });

            NotifyExecuted(notifyId);
        }

        private void Execute(RemovePanelAction action, int notifyId)
        {
            Messenger.Send(new UnloadPanelMessage() { PanelHolderId = action.PanelHolder });
            NotifyExecuted(notifyId);
        }

        private void Execute(LinearPositionLinkAction action, int notifyId)
        {
            if (IsDynamic) ExecuteDynamic(action, notifyId);
            else ExecuteStatic(action, notifyId);
        }

        private void ExecuteStatic(LinearPositionLinkAction action, int notifyId)
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

        private void ExecuteDynamic(LinearPositionLinkAction action, int notifyId)
        {
            Messenger.Send(new GetLinkMessage()
            {
                Id = action.LinkId,
                SetLink = (link) =>
                {
                    LinkMovementManager.Add(action.LinkId, action.RequestedPosition, action.Duration, notifyId);
                }
            });            
        }

        private void Execute(TwoPositionLinkAction action, int notifyId)
        {
            if (IsDynamic) ExecuteDynamic(action, notifyId);
            else ExecuteStatic(action, notifyId);
        }

        private void ExecuteStatic(TwoPositionLinkAction action, int notifyId)
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

        private void ExecuteDynamic(TwoPositionLinkAction action, int notifyId)
        {
            Messenger.Send(new GetLinkMessage()
            {
                Id = action.LinkId,
                SetLink = (link) =>
                {
                    var state = action.RequestedState == MachineSteps.Models.Enums.TwoPositionLinkActionRequestedState.On;
                    if (!(link as IPneumaticLinkViewModel).ChangeStatus(state, notifyId)) NotifyExecuted(notifyId);
                    // se il cambiamento non è avvenuto è perché lo stato del link era già quello desiderato => va notificata l'avvenuta esecuzione
                }
            });
        }

        private void Execute(LoadToolAction action, int notifyId)
        {
            Messenger.Send(new MoveToolRequestMessage()
            {
                Source = action.ToolSource,
                Sink = action.ToolSink
            });

            NotifyExecuted(notifyId);
        }

        private void Execute(UnloadToolAction action, int notifyId)
        {
            Messenger.Send(new MoveToolRequestMessage()
            {
                Source = action.ToolSource,
                Sink = action.ToolSink
            });

            NotifyExecuted(notifyId);
        }

        private void Execute(LinearPositionLinkGantryOnAction action, int notifyId)
        {
            Messenger.Send(new GantryMessage()
            {
                Master = action.MasterId,
                Slave = action.SlaveId,
                State = true
            });

            NotifyExecuted(notifyId);
        }

        private void Execute(LinearPositionLinkGantryOffAction action, int notifyId)
        {
            Messenger.Send(new GantryMessage()
            {
                Master = action.MasterId,
                Slave = action.SlaveId,
                State = false
            });

            NotifyExecuted(notifyId);
        }

        private void Execute(LinearInterpolatedPositionLinkAction action, int notifyId)
        {
            if (IsDynamic) ExecuteDynamic(action, notifyId);
            else ExecuteStatic(action, notifyId);
        }

        private void ExecuteStatic(LinearInterpolatedPositionLinkAction action, int notifyId)
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

        private void ExecuteDynamic(LinearInterpolatedPositionLinkAction action, int notifyId)
        {
            bool isFirst = true;

            foreach (var p in action.Positions)
            {
                LinkMovementManager.Add(_interpolationGroupId, p.LinkId, p.RequestPosition, action.Duration, isFirst ? notifyId : 0);
                isFirst = false;
            }

            _interpolationGroupId++;
        }

        private void Execute(ArcInterpolatedPositionLinkAction action, int notifyId)
        {
            if (IsDynamic) ExecuteDynamic(action, notifyId);
            else ExecuteStatic(action, notifyId);
        }

        private void ExecuteStatic(ArcInterpolatedPositionLinkAction action, int notifyId)
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

        private void ExecuteDynamic(ArcInterpolatedPositionLinkAction action, int notifyId)
        {
            bool isFirst = true;

            foreach (var item in action.Components)
            {
                var component = item.Type == ArcInterpolatedPositionLinkAction.ArcComponent.ArcComponentType.X ? ArcComponent.X : ArcComponent.Y;
                var data = new ArcComponentData
                {
                    GroupId = _interpolationGroupId,
                    StartAngle = action.StartAngle,
                    Angle = action.Angle,
                    Radius = action.Radius,
                    CenterCoordinate = item.CenterCoordinate,
                    Component = component
                };

                Messenger.Send(new GetLinkMessage()
                {
                    Id = item.LinkId,
                    SetLink = (link) =>
                    {
                        LinkMovementManager.Add(item.LinkId, item.TargetCoordinate, action.Duration, data, isFirst ? notifyId : 0);
                        isFirst = false;
                    }
                });
            }

            _interpolationGroupId++;
        }

        private void Execute(InjectAction action, int notifyId)
        {
            Messenger.Send(new InjectMessage() { InjectorId = action.InjectorId });

            NotifyExecuted(notifyId, action.Duration);            
        }

        private void Execute(TurnOffInverterAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        private void Execute(TurnOnInverterAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        private void Execute(UpdateRotationSpeedAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        private void Execute(ChannelWaiterAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        private void Execute(NotOperationAction action, int notifyId)
        {
            NotifyExecuted(notifyId);
        }

        private void NotifyExecuted(int notifyId)
        {
            if (notifyId > 0) Messenger.Send(new ActionExecutedMessage() { Id = notifyId });
        }

        private void NotifyExecuted(int notifyId, double duration)
        {
            if (notifyId > 0)
            {
                if (IsDynamic)
                {
                    Task.Delay(TimeSpan.FromSeconds(duration))
                        .ContinueWith((t) =>
                        {
                            NotifyExecuted(notifyId);
                        });
                }
                else
                {
                    NotifyExecuted(notifyId);
                }
            }
        }
    }
}
