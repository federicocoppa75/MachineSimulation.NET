using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages.Links;
using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using MVMIoc = Machine.ViewModels.Ioc;

namespace Machine.Steps.ViewModels.Extensions.Models
{
    static class LinkActionExtentions
    {
        public static ILinkViewModel GetLink(this LinkAction action) => action.GetLink(action.LinkId);
        //{
        //    ILinkViewModel link = null;

        //    MVMIoc.SimpleIoc<IMessenger>.GetInstance().Send(new GetLinkMessage()
        //    {
        //        Id = action.LinkId,
        //        SetLink = (lnk) => link = lnk
        //    });

        //    return link;
        //}

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
