using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface ICameraControl
    {
        void SetPosition(double x, double y, double z);
        void SetLookDirection(double x, double y, double z);
        void SetUpDirection(double x, double y, double z);
    }
}
