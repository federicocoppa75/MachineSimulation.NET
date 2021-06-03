using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class PanelViewModel : ElementViewModel, IMovablePanel, IDisposable
    {        
        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double SizeZ { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double CenterZ { get; set; }

        private double _offsetX;
        public double OffsetX
        {
            get => _offsetX; 
            set => Set(ref _offsetX, value, nameof(OffsetX));
        }

        private IHookablePanel _hookingHandle;
        public IHookablePanel HookingHandle => (_hookingHandle ?? (_hookingHandle = new HookablePanelHandle(this)));

        public PanelViewModel()
        {
            Messenger.Register<GetPanelMessage>(this, OnGetPanelMessage);
        }

        private void OnGetPanelMessage(GetPanelMessage msg) => msg.SetPanel(this);

        #region IDisposable
        private bool _disposed = false;

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Dispose managed state (managed objects).
                Messenger.Unregister(this);
            }

            _disposed = true;
        }
        #endregion
    }

    class HookablePanelHandle : IHookablePanel
    {
        private List<IPanelHooker> _hookers = new List<IPanelHooker>();
        private IMovablePanel _panel;
        private double _attachValue;
        private double _attachPosition;

        public HookablePanelHandle(IMovablePanel panel)
        {
            _panel = panel;
        }

        public void Hook(IPanelHooker hooker)
        {
            if (_hookers.Count == 0)
            {
                AttachHooker(hooker);
            }

            _hookers.Add(hooker);
        }

        public void Unhook(IPanelHooker hooker)
        {
            if (_hookers.Count > 0)
            {
                bool detached = false;

                if (ReferenceEquals(_hookers[0], hooker))
                {
                    DetachHooker(hooker);
                    detached = true;
                }

                _hookers.Remove(hooker);

                if (detached && (_hookers.Count > 0))
                {
                    AttachHooker(_hookers[0]);
                }
            }
        }

        private void AttachHooker(IPanelHooker hooker)
        {
            var link = hooker.Link;

            if (link != null)
            {
                _attachValue = link.Value;
                _attachPosition = _panel.OffsetX;
                link.ValueChanged += OnLinkValueChanged;
            }
        }

        private void DetachHooker(IPanelHooker hooker)
        {
            var link = hooker.Link;

            if (link != null)
            {
                link.ValueChanged -= OnLinkValueChanged;
            }

        }

        private void OnLinkValueChanged(object sender, double e) => _panel.OffsetX = (e - _attachValue) + _attachPosition;
    }
}
