using Machine.Data.Base;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Probing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Machine.ViewModels.Probing
{
    public class DistanceProbeViewModel : BaseProbeViewModel, IProbe, IProbeDistance
    {
        #region IProbe
        public int ProbeId { get; set; }

        private double _x;
        public double X
        {
            get => _x;
            set => Set(ref _x, value, nameof(X));
        }

        private double _y;
        public double Y
        {
            get => _y;
            set => Set(ref _y, value, nameof(Y));
        }

        private double _z;
        public double Z
        {
            get => _z;
            set => Set(ref _z, value, nameof(Z));
        }
        #endregion

        #region IProbeDistance
        private IProbePoint _master;
        public IProbePoint Master 
        { 
            get => _master; 
            set
            {
                if (_master != null) Detach(_master);
                if (Set(ref _master, value, nameof(Master)) && (_master != null)) Attach(_master);
            }
        }

        private IProbePoint _slave;
        public IProbePoint Slave 
        { 
            get => _slave; 
            set
            {
                if (_slave != null) Detach(_slave);
                if (Set(ref _slave, value, nameof(Slave)) && (_slave != null)) Attach(_slave);
            }
        }
        #endregion

        #region implementation
        private void Attach(IProbePoint probePoint)
        {
            if(probePoint is IProbePointChangable ppc) ppc.PointChanged += OnPointChanged;

            SetValue();
        }

        private void Detach(IProbePoint probePoint)
        {
            if (probePoint is IProbePointChangable ppc) ppc.PointChanged -= OnPointChanged;
        }

        public void Detach()
        {
            Master = null;
            Slave = null;
        }

        private void OnPointChanged(object sender, double e) => SetValue();

        private void SetValue()
        {
            if((Master != null) && (Slave != null))
            {
                X = Slave.X - Master.X;
                Y = Slave.Y - Master.Y;
                Z = Slave.Z - Master.Z;
            }
        }
        #endregion
    }
}
