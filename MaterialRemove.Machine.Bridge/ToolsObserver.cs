using System;
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
            //var tTools = t.Transform();

            t.TransformAndApplay();

            //for (int i = 0; i < _tools.Count; i++)
            //{
            //    //ApplyTool(tTools[i], _tools[i]);

            //    //var ta = new ToolActionData()
            //    //{
            //    //    X = (float)tt.Point.X,
            //    //    Y = (float)tt.Point.Y,
            //    //    Z = (float)tt.Point.Z,
            //    //    Orientation = ToOrientatio(tt.Direction),
            //    //    Length = (float)tt.Length,
            //    //    Radius = (float)tt.Radius
            //    //};

            //    //_panel.ApplyAction(ta);
            //}
        }

        //private void ApplyTool(ToolPosition toolPosition, IToolElement toolElement)
        //{
        //    var tool = (toolElement as ToolViewModel).Tool;

        //    if (tool is DiskTool dt) ApplyTool(toolPosition, dt);
        //    else if (tool is DiskOnConeTool doct) ApplyTool(toolPosition, doct);
        //    else if (tool is PointedTool pt) ApplyTool(toolPosition, pt);
        //    else if (tool is CountersinkTool ct) ApplyTool(toolPosition, ct);
        //    else ApplyTool(toolPosition);
        //}

        //private void ApplyTool(ToolPosition toolPosition)
        //{
        //    var ta = new ToolActionData()
        //    {
        //        X = (float)toolPosition.Point.X,
        //        Y = (float)toolPosition.Point.Y,
        //        Z = (float)toolPosition.Point.Z,
        //        Orientation = ToOrientatio(toolPosition.Direction),
        //        Length = (float)toolPosition.Length,
        //        Radius = (float)toolPosition.Radius
        //    };

        //    _panel.ApplyAction(ta);
        //}

        private void ApplyTool(ToolPosition toolPosition, CountersinkTool ct)
        {
            //var p = new Point3D
            //var p1 = toolPosition.Point + toolPosition.Direction * (ct.Length1 + ct.Length2);
            //var ta = new ToolConeActionData()
            //{

            //};

            //_panel.ApplyAction(ta);
        }

        //private void ApplyTool(ToolPosition toolPosition, PointedTool pt)
        //{
        //    throw new NotImplementedException();
        //}

        //private void ApplyTool(ToolPosition toolPosition, DiskTool dt)
        //{
        //    throw new NotImplementedException();
        //}

        private Task ApplyToolsAsync()
        {
            return Task.Run(async () =>
            {
                var tasks = new List<Task>();
                var t = MVMIoc.SimpleIoc<IToolToPanelTransformerFactory>.GetInstance().GetTransformer(_panel, _tools);
                //var tTools = await t.TransformAsync();

                //foreach (var tt in tTools)
                //{
                //    var ta = new ToolActionData()
                //    {
                //        X = (float)tt.Point.X,
                //        Y = (float)tt.Point.Y,
                //        Z = (float)tt.Point.Z,
                //        Orientation = ToOrientatio(tt.Direction),
                //        Length = (float)tt.Length,
                //        Radius = (float)tt.Radius
                //    };

                //    tasks.Add(_panel.ApplyActionAsync(ta));
                //}

                //await Task.WhenAll(tasks);

                await t.TransformAndApplayAsync();
            });
        }

        //private Orientation ToOrientatio(Vector direction)
        //{
        //    var xIsNull = IsNull(direction.X);
        //    var yIsNull = IsNull(direction.Y);
        //    var zIsNull = IsNull(direction.Z);

        //    if (xIsNull && yIsNull && !zIsNull)
        //    {
        //        return (direction.Z > 0.0) ? Orientation.ZPos : Orientation.ZNeg;
        //    }
        //    else if (xIsNull && !yIsNull && zIsNull)
        //    {
        //        return (direction.Y > 0.0) ? Orientation.YPos : Orientation.YNeg;
        //    }
        //    else if (!xIsNull && yIsNull && zIsNull)
        //    {
        //        return (direction.X > 0.0) ? Orientation.XPos : Orientation.XNeg;
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //private static bool IsNull(double value, double tolerance = 0.001) => (value < tolerance) && (value > -tolerance);

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
