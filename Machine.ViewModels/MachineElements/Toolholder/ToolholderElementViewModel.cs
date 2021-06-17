using Machine.Data.Base;
using Machine.Data.Enums;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Tools;
using Machine.ViewModels.Messages.Tooling;

namespace Machine.ViewModels.MachineElements.Toolholder
{
    public abstract class ToolholderElementViewModel : ElementViewModel, IToolholderElement
    {
        private static Color _toolColor = new Color() { A = 255, B = 255 };
        private static Color _coneColor = new Color() { A = 255, B = 128, G = 128, R = 128 };

        public int ToolHolderId { get; set; }
        public abstract ToolHolderType ToolHolderType { get; }
        public Vector Position { get; set; }
        public Vector Direction { get; set; }
        public bool ActiveTool { get; set; }

        public ToolholderElementViewModel() : base()
        {
            Messenger.Register<LoadToolMessage>(this, OnLoadToolMessage);
            Messenger.Register<UnloadToolMessage>(this, OnUnloadToolMessage);
            Messenger.Register<UnloadAllToolMessage>(this, OnUnloadAllToolMessage);
            Messenger.Register<AngularTransmissionLoadMessage>(this, OnAngularTransmissionLoadMessage);
        }

        protected virtual void OnAngularTransmissionLoadMessage(AngularTransmissionLoadMessage msg)
        {
            if (msg.ToolHolder == ToolHolderId)
            {
                var vm = new AngularTransmissionViewModel()
                {
                    Name = msg.AngularTransmission.Name,
                    Tool = msg.AngularTransmission,
                    IsVisible = true,
                    Parent = this
                };

                msg.AppendSubSpindle((p, v, t) => vm.AppendSubSpindle(p, v, t));

                Children.Add(vm);
            }
        }

        protected virtual void OnLoadToolMessage(LoadToolMessage msg)
        {
            if (msg.ToolHolder == ToolHolderId)
            {
                var vm = new ToolViewModel()
                {
                    Name = msg.Tool.Name,
                    Tool = msg.Tool,
                    Color = _toolColor,
                    ConeColor = _coneColor,
                    IsVisible = true,
                    Parent = this
                };

                Children.Add(vm);               
            }
        }

        protected virtual void OnUnloadToolMessage(UnloadToolMessage msg)
        {
            if (msg.ToolHolder == ToolHolderId)
            {
                UnloadTool();
            }
        }

        protected virtual void OnUnloadAllToolMessage(UnloadAllToolMessage msg) => UnloadTool();

        private void UnloadTool()
        {
            foreach (var item in Children)
            {
                ForceDeactivation(item);
                item.Parent = null;
            }
            Children.Clear();
        }

        protected void AttachActivator()
        {
            IMachineElement p = this;

            while (p != null)
            {
                if (p.LinkToParent is IPneumaticLinkViewModel plink)
                {
                    plink.StateChanged += OnPneumaticLinkStateChanged;
                    break;
                }

                p = p.Parent;
            }
        }

        protected virtual void OnPneumaticLinkStateChanged(object sender, bool e) => ManageActivation(this, e);

        private void ManageActivation(IMachineElement me, bool value)
        {
            if(me is IToolElement tool)
            {
                if(value) GetInstance<IToolObserverProvider>().Observer.Register(tool);
                else GetInstance<IToolObserverProvider>().Observer.Unregister(tool);
            }
            else
            {
                foreach (var item in me.Children)
                {
                    ManageActivation(item, value);
                }
            }
        }

        private void ForceDeactivation(IMachineElement me)
        {
            if (me is IToolElement tool)
            {
                GetInstance<IToolObserverProvider>().Observer.Unregister(tool);
            }
            else
            {
                foreach (var item in me.Children)
                {
                    ForceDeactivation(me);
                }
            }
        }
    }
}
