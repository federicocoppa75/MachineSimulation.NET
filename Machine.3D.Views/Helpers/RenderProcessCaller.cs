using HelixToolkit.Wpf.SharpDX.Controls;
using Machine.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Helpers
{
    class RenderProcessCaller : IProcessCaller, IDisposable
    {
        object _lockObj = new object();
        int _references;
        CompositionTargetEx _compositionTarget;

        public bool Enable 
        {
            get => _compositionTarget != null;
            set => throw new NotSupportedException();
        }

        private event EventHandler<DateTime> _processRequest;
        public event EventHandler<DateTime> ProcessRequest
        {
            add
            {
                lock (_lockObj)
                {
                    ManageRegister();

                    _processRequest += value;
                    _references++;
                }
            }
            remove
            {
                lock(_lockObj)
                {
                    _processRequest -= value;
                    _references--;

                    if(_references < 1) ManageUnregister();
                }
            }
        }

        private void ManageRegister()
        {
            if (_compositionTarget == null)
            {
                _compositionTarget = new CompositionTargetEx();
                _compositionTarget.Rendering += OnRendering;
            }
        }

        private void ManageUnregister()
        {
            if (_compositionTarget != null)
            {
                _compositionTarget.Rendering -= OnRendering;
                _compositionTarget.Dispose();
                _compositionTarget = null;
            }
        }

        private void OnRendering(object sender, System.Windows.Media.RenderingEventArgs e) => _processRequest?.Invoke(this, DateTime.Now);

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

            if(disposing)
            {
                lock(_lockObj)
                {
                    ManageUnregister();
                }
            }

            _disposed = true;
        }
        #endregion
    }
}
