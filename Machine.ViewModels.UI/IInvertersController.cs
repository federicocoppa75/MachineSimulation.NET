using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface IInvertersController
    {
        void TurnOn(string name, int value);
        void TurnOff();
        void Change(int value);
    }
}
