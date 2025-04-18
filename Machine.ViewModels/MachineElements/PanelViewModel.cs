﻿using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public class PanelViewModel : ElementViewModel, IPanelElement, IMovablePanel, IInsertionsSink, IDisposable
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
            set
            {
                if(Set(ref _offsetX, value, nameof(OffsetX)))
                {
                    ValueChanged?.Invoke(this, _offsetX);
                }
            }
        }

        private IHookablePanel _hookingHandle;
        public IHookablePanel HookingHandle => (_hookingHandle ?? (_hookingHandle = new HookablePanelHandle(this)));

        public event EventHandler<double> ValueChanged;

        public PanelViewModel()
        {
            Messenger.Register<GetPanelMessage>(this, OnGetPanelMessage);
            RegisterAsInsertionSink();
        }

        private void OnGetPanelMessage(GetPanelMessage msg) => msg.SetPanel(this);

        private void RegisterAsInsertionSink()
        {
            var sp = GetInstance<IInsertionsSinkProvider>();

            if (sp != null) sp.InsertionsSink = this;
        }

        private void UnregisterAsInsertionSink()
        {
            var sp = GetInstance<IInsertionsSinkProvider>();

            if (sp != null) sp.InsertionsSink = null;
        }

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects).
                Messenger.Unregister(this);
                UnregisterAsInsertionSink();
            }
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
