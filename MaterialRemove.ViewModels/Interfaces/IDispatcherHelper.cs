using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels.Interfaces
{
    // ho ridefinito l'interfaccia IDispatcherHelper per evitare di introdurre la dipendeza da Machine.ViewModel.UI
    public interface IDispatcherHelper
    {
        void CheckBeginInvokeOnUi(Action action);
    }
}
