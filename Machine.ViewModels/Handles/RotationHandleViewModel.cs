using Machine.Data.Base;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Handles;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Handles
{
    public class RotationHandleViewModel : BaseHandleViewModel, IRotationHandle
    {
        IElementRotator _rotator;

        #region IRotationHandle
        public Interfaces.Handles.Type Type { get; set; }

        public void Rotate(double angle)
        {
            if (_rotator == null) throw new InvalidOperationException("Call StartRotate before!");

            GetRotationDirection(out double x, out double y, out double z);
            _rotator.Rotate(x, y, z, angle);
        }

        public void StartRotate()
        {
            _rotator = GetInstance<IElementRotatorFactory>().Create(Parent.Parent);
        }
        #endregion

        protected override string GetName() => $"Rot handle {Type}";

        private void GetRotationDirection(out double x, out double y, out double z)
        {
            x = 0.0;
            y = 0.0;
            z = 0.0;

            switch (Type)
            {
                case Interfaces.Handles.Type.X:
                    x = 1.0;
                    break;
                case Interfaces.Handles.Type.Y:
                    y = 1.0;
                    break;
                case Interfaces.Handles.Type.Z:
                    z = 1.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
