using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages.Links;
using Machine.ViewModels.Messaging;
using MachineSteps.Models.Actions;
using MVMIoc = Machine.ViewModels.Ioc;

namespace Machine.Steps.ViewModels.Extensions.Models
{
    static class LinkActionExtentions
    {
        public static ILinkViewModel GetLink(this LinkAction action) => action.GetLink(action.LinkId);

        public static ILinkViewModel GetLink(this BaseAction action, int linkId)
        {
            ILinkViewModel link = null;

            MVMIoc.SimpleIoc<IMessenger>.GetInstance().Send(new GetLinkMessage()
            {
                Id = linkId,
                SetLink = (lnk) => link = lnk
            });

            return link;
        }
    }
}
