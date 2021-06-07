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
        CompositionTargetEx _compositionTarget;

        private bool _enable;
        public bool Enable 
        {
            get => _enable;
            set
            {
                if(_enable != value)
                {
                    _enable = value;

                    if(_enable && (_compositionTarget == null))
                    {
                        _compositionTarget = new CompositionTargetEx();
                        _compositionTarget.Rendering += OnRendering;
                    }
                    else if(!_enable && (_compositionTarget != null))
                    {
                        _compositionTarget.Rendering -= OnRendering;
                        _compositionTarget.Dispose();
                        _compositionTarget = null;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            } 
        }

        public event EventHandler<DateTime> ProcessRequest;

        private void OnRendering(object sender, System.Windows.Media.RenderingEventArgs e) => ProcessRequest?.Invoke(this, DateTime.Now);

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
                Enable = false;
            }

            _disposed = true;
        }
        #endregion
    }
}
