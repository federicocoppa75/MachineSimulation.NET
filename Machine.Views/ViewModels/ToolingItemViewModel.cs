using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Messages.Tooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.Views.ViewModels
{
    internal class ToolingItemViewModel : BaseViewModel
    {
        public IToolholderElement Toolholder { get; set; }

        public string ToolName 
        { 
            get
            {
                if((Toolholder.Children != null) && (Toolholder.Children.Count > 0))
                {
                    var t = Toolholder.Children.FirstOrDefault(o => (o is IToolElement) || (o is IAngularTransmission));

                    return (t != null) ? t.Name : string.Empty;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public ToolingItemViewModel() : base()
        {
            Messenger.Register<LoadToolMessage>(this, OnLoadToolMessage);
            Messenger.Register<UnloadToolMessage>(this, OnUnloadToolMessage);
            Messenger.Register<UnloadAllToolMessage>(this, OnUnloadAllToolMessage);
            Messenger.Register<AngularTransmissionLoadMessage>(this, OnAngularTransmissionLoadMessage);
        }

        private void OnLoadToolMessage(LoadToolMessage msg)
        {
            if(msg.ToolHolder == Toolholder.ToolHolderId) RisePropertyChanged(nameof(ToolName));
        }

        private void OnUnloadToolMessage(UnloadToolMessage msg)
        {
            if (msg.ToolHolder == Toolholder.ToolHolderId) RisePropertyChanged(nameof(ToolName));
        }

        private void OnUnloadAllToolMessage(UnloadAllToolMessage msg) => RisePropertyChanged(nameof(ToolName));

        private void OnAngularTransmissionLoadMessage(AngularTransmissionLoadMessage msg)
        {
            if (msg.ToolHolder == Toolholder.ToolHolderId) RisePropertyChanged(nameof(ToolName));
        }
    }
}
