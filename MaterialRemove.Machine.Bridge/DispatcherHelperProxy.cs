using System;
using System.Collections.Generic;
using System.Text;
using MRVMI = MaterialRemove.ViewModels.Interfaces;
using MVMIoc = Machine.ViewModels.Ioc;
using MVMUI = Machine.ViewModels.UI;

namespace MaterialRemove.Machine.Bridge
{
    internal class DispatcherHelperProxy : MRVMI.IDispatcherHelper
    {
        public void CheckBeginInvokeOnUi(Action action)
        {
            MVMIoc.SimpleIoc<MVMUI.IDispatcherHelper>.GetInstance().CheckBeginInvokeOnUi(() =>
            {
                action?.Invoke();
            });
        }
    }
}
