﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Machine.Data.Base;
using Machine.Data.Tools;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Tools;
using Machine.ViewModels.MachineElements;
using MaterialRemove.Interfaces;
using MDE = Machine.Data.Enums;
using MVMIoc = Machine.ViewModels.Ioc;

namespace MaterialRemove.Machine.Bridge
{
    class ToolsObserver : IToolsObserver, IDisposable
    {
        List<IToolElement> _tools = new List<IToolElement>();
        Dictionary<int, int> _links = new Dictionary<int, int>();
        private int _shiftHappens = 0;
        DateTime _lastProcess = DateTime.MinValue;
        private IProcessCaller _processCaller;
        private int _processTimestamp = 100;
        private int _processing = 0;
        private PanelViewModel _panel;
        private object _toolsLockObj = new object();
        private IMaterialRemoveData _materialRemoveData;

        private int ToolsCount 
        {
            get
            {
                lock(_toolsLockObj)
                {
                    return _tools.Count;
                }
            }
        }

        public ToolsObserver(PanelViewModel panel)
        {
            _materialRemoveData = MVMIoc.SimpleIoc<IMaterialRemoveData>.GetInstance();
            _panel = panel;
            _panel.ValueChanged += OnPanelOffsetValueChanged;
            _processCaller = MVMIoc.SimpleIoc<IProcessCaller>.GetInstance();
            _processCaller.ProcessRequest += OnProcessRequest;
        }

        private void OnProcessRequest(object sender, DateTime e)
        {
            if(_materialRemoveData.Enable &&
                (ToolsCount > 0) && 
                ((_lastProcess == DateTime.MinValue) || ((e - _lastProcess >= TimeSpan.FromMilliseconds(_processTimestamp))) &&  
                Interlocked.CompareExchange(ref _processing, 1, 0) == 0))
            {
                _lastProcess = e;

                Task.Run(async () =>
                {
                    if (Interlocked.CompareExchange(ref _shiftHappens, 0, 1) == 1)
                    {
                        //ApplyTools();
                        await ApplyToolsAsync();
                    }

                    Interlocked.Exchange(ref _processing, 0);                    
                });                
            }
        }

        private void ApplyTools()
        {
            var t = MVMIoc.SimpleIoc<IToolToPanelTransformerFactory>.GetInstance().GetTransformer(_panel, _tools);

            t.TransformAndApplay();
        }

        private Task ApplyToolsAsync()
        {
            return Task.Run(async () =>
            {
                var tasks = new List<Task>();
                var t = MVMIoc.SimpleIoc<IToolToPanelTransformerFactory>.GetInstance().GetTransformer(_panel, _tools);

                await t.TransformAndApplayAsync();
            });
        }

        public void Register(IToolElement tool)
        {
            lock (_toolsLockObj)
            {
                _tools.Add(tool);
                RegisterLinks(tool);
            }
        }

        public void Unregister(IToolElement tool)
        {
            lock(_toolsLockObj)
            {
                if (_tools.Contains(tool))
                {
                    UnregisterLinks(tool);
                    _tools.Remove(tool);
                }
            }
        }

        private void RegisterLinks(IToolElement tool)
        {
            IMachineElement me = tool;

            while(me.Parent != null)
            {
                if((me.LinkToParent != null) && (me.LinkToParent.MoveType == MDE.LinkMoveType.Linear))
                {
                    if(_links.TryGetValue(me.LinkToParent.Id, out int n))
                    {
                        _links[me.LinkToParent.Id] = n + 1;
                    }
                    else
                    {
                        _links.Add(me.LinkToParent.Id, 1);
                        me.LinkToParent.ValueChanged += OnLinkValueChanged;
                    }
                }

                me = me.Parent;
            }
        }

        private void UnregisterLinks(IToolElement tool)
        {
            IMachineElement me = tool;

            while (me.Parent != null)
            {
                if ((me.LinkToParent != null) && (me.LinkToParent.MoveType == MDE.LinkMoveType.Linear))
                {
                    if (_links.TryGetValue(me.LinkToParent.Id, out int n))
                    {
                        if(n <= 1)
                        {
                            me.LinkToParent.ValueChanged -= OnLinkValueChanged;
                            _links.Remove(me.LinkToParent.Id);
                        }
                        else
                        {
                            _links[me.LinkToParent.Id] = n - 1;
                        }
                    }
                }

                me = me.Parent;
            }
        }

        private void OnLinkValueChanged(object sender, double e) => Interlocked.Exchange(ref _shiftHappens, 1);

        private void OnPanelOffsetValueChanged(object sender, double e) => Interlocked.Exchange(ref _shiftHappens, 1);


        #region IDisposable
        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Dispose managed state (managed objects).
                _processCaller.ProcessRequest -= OnProcessRequest;
                _processCaller = null;
                _panel.ValueChanged -= OnPanelOffsetValueChanged;
                foreach (var tool in _tools) UnregisterLinks(tool);
                _tools.Clear();
            }

            _disposed = true;
        }
        #endregion
    }
}
