using Machine.Steps.ViewModels.Interfaces;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages.Links;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;

namespace Machine.Steps.ViewModels.Extensions
{
    public class DurationProvider : IDurationProvider
    {
        public double GetDuration(BaseAction action)
        {
            double result = 0.0;

            if (action is IGradualLinkAction gla) result = gla.Duration;
            else if (action is TwoPositionLinkAction tpla) result = GetDuration(tpla);
            else if (action is InjectAction ia) result = ia.Duration;
            else if (action is TurnOffInverterAction toffa) result = toffa.Duration;
            else if (action is TurnOnInverterAction tona) result = tona.Duration;
            else if (action is UpdateRotationSpeedAction ursa) result = ursa.Duration;

            return result;
        }

        public static double GetDuration(TwoPositionLinkAction action)
        {
            double result = 0.0;
            var messenger = Machine.ViewModels.Ioc.SimpleIoc<IMessenger>.GetInstance();

            messenger.Send(new GetLinkMessage()
            {
                Id = action.LinkId,
                SetLink = (link) =>
                {
                    var plink = link as IPneumaticLinkViewModel;

                    result = (action.RequestedState == TwoPositionLinkActionRequestedState.On) ? plink.TOn : plink.TOff;
                }
            });

            return result;
        }
    }
}
