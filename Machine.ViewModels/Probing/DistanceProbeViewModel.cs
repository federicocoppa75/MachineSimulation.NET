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
    public class DistanceProbeViewModel : BaseViewModel, IMachineElement, IViewElementData, IProbe, IProbeDistance
    {
        #region IProbe
        public int ProbeId { get; set; }

        public double X => Master.X - Slave.X;

        public double Y => Master.Y - Slave.Y;

        public double Z => Master.Z - Slave.Z;
        #endregion

        #region IProbeDistance
        public IProbePoint Master { get; set; }
        public IProbePoint Slave { get; set; }
        #endregion

        #region IViewElementData
        public bool IsVisible { get; set; }
        public bool IsSelected { get; set; }
        public string PostEffects { get; set; }
        public bool IsExpanded { get; set; }
        #endregion

        #region IMachineElement
        public int MachineElementID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get; set; }
        public string ModelFile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color Color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Matrix Transformation { get; set; }

        public ICollection<IMachineElement> Children { get; private set; } = new ObservableCollection<IMachineElement>();

        public ILinkViewModel LinkToParent { get => null; set => throw new NotImplementedException(); }
        public IMachineElement Parent { get; set; }
        #endregion
    }
}
