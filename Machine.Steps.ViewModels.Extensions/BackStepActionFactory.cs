using Machine.Steps.ViewModels.Extensions.Models;
using Machine.Steps.ViewModels.Interfaces;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.Steps.ViewModels.Extensions
{
    public class BackStepActionFactory : IBackStepActionFactory
    {
        public BaseAction Create(BaseAction action)
        {
            BaseAction ba = null;

            if (action is AddPanelAction apa) ba = CreateBackStepAction(apa);
            else if (action is LinearPositionLinkAction lpla) ba = CreateBackStepAction(lpla);
            else if (action is LoadToolAction lta) ba = CreateBackStepAction(lta);
            else if (action is TwoPositionLinkAction tpla) ba = CreateBackStepAction(tpla);
            else if (action is UnloadToolAction uta) ba = CreateBackStepAction(uta);
            else if (action is LinearPositionLinkGantryOnAction lplgona) ba = CreateBackStepAction(lplgona);
            else if (action is LinearPositionLinkGantryOffAction lplgoffa) ba = CreateBackStepAction(lplgoffa);
            else if (action is LinearInterpolatedPositionLinkAction lipla) ba = CreateBackStepAction(lipla);
            else if (action is ArcInterpolatedPositionLinkAction aipla) ba = CreateBackStepAction(aipla);
            else if (action is TurnOffInverterAction toffia) ba = CreateBackStepAction(toffia);
            else if (action is TurnOnInverterAction tonia) ba = CreateBackStepAction(tonia);
            else if (action is UpdateRotationSpeedAction ursa) ba = CreateBackStepAction(ursa);

            if (ba != null)
            {
                ba.Id = -action.Id;
                ba.Name = $"{action.Name}(bk)";
                ba.Description = $"{action.Description} (back step action)";
            }

            return ba;
        }

        private BaseAction CreateBackStepAction(AddPanelAction action)
        {
            return new RemovePanelAction()
            {
                CornerReference = action.CornerReference,
                PanelHolder = action.PanelHolder,
                PanelId = action.PanelId
            };
        }

        private BaseAction CreateBackStepAction(LinearPositionLinkAction action)
        {
            return new LinearPositionLinkLazyAction() { LinkId = action.LinkId };
        }

        private BaseAction CreateBackStepAction(LoadToolAction action)
        {
            return new UnloadToolAction()
            {
                ToolSink = action.ToolSource,
                ToolSource = action.ToolSink
            };
        }

        private BaseAction CreateBackStepAction(TwoPositionLinkAction action)
        {
            return new TwoPositionLinkLazyAction() { LinkId = action.LinkId };
        }

        private BaseAction CreateBackStepAction(UnloadToolAction action)
        {
            return new LoadToolAction()
            {
                ToolSink = action.ToolSource,
                ToolSource = action.ToolSink
            };
        }

        private BaseAction CreateBackStepAction(LinearPositionLinkGantryOnAction action)
        {
            return new LinearPositionLinkGantryOffAction()
            {
                MasterId = action.MasterId,
                SlaveId = action.SlaveId
            };
        }

        private BaseAction CreateBackStepAction(LinearPositionLinkGantryOffAction action)
        {
            return new LinearPositionLinkGantryOnAction()
            {
                MasterId = action.MasterId,
                SlaveId = action.SlaveId,
                SlaveUnhooked = action.SlaveUnhooked
            };
        }

        private BaseAction CreateBackStepAction(LinearInterpolatedPositionLinkAction action)
        {
            var ba = new LinearInterpolatedPositionLinkLazyAction()
            {
                Duration = action.Duration,
                Positions = new List<LinearInterpolatedPositionLinkAction.PositionItem>()
            };

            for (int i = 0; i < action.Positions.Count(); i++)
            {
                ba.Positions.Add(new LinearInterpolatedPositionLinkAction.PositionItem() { LinkId = action.Positions[i].LinkId });
            }

            return ba;
        }

        private BaseAction CreateBackStepAction(ArcInterpolatedPositionLinkAction action)
        {
            var ba = new ArcInterpolatedPositionLinkLazyAction()
            {
                Direction = action.Direction == ArcInterpolatedPositionLinkAction.ArcDirection.CW ? ArcInterpolatedPositionLinkAction.ArcDirection.CCW : ArcInterpolatedPositionLinkAction.ArcDirection.CW,
                Duration = action.Duration,
                Radius = action.Radius,
                StartAngle = action.StartAngle,
                EndAngle = action.EndAngle,
                Angle = -action.Angle,
                Components = new List<ArcInterpolatedPositionLinkAction.ArcComponent>()
            };

            for (int i = 0; i < action.Components.Count(); i++)
            {
                ba.Components.Add(new ArcInterpolatedPositionLinkAction.ArcComponent()
                {
                    LinkId = action.Components[i].LinkId,
                    CenterCoordinate = action.Components[i].CenterCoordinate,
                    Type = action.Components[i].Type
                });
            }

            return ba;
        }

        private BaseAction CreateBackStepAction(TurnOffInverterAction action)
        {
            if (action.RotationSpeed > 0)
            {
                return new TurnOnInverterAction()
                {
                    Head = action.Head,
                    Order = action.Order,
                    RotationSpeed = action.RotationSpeed
                };
            }
            else
            {
                return null;
            }
        }

        private BaseAction CreateBackStepAction(TurnOnInverterAction action)
        {
            return new TurnOffInverterAction()
            {
                Head = action.Head,
                Order = action.Order,
                RotationSpeed = action.RotationSpeed
            };
        }

        private BaseAction CreateBackStepAction(UpdateRotationSpeedAction action)
        {
            return new UpdateRotationSpeedAction()
            {
                NewRotationSpeed = action.OldRotationSpeed,
                OldRotationSpeed = action.NewRotationSpeed,
                Duration = action.Duration
            };
        }
    }
}
