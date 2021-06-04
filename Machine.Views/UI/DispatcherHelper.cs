using Machine.ViewModels.UI;
using System;
using System.Windows.Threading;

namespace Machine.Views.UI
{
    public class DispatcherHelper : IDispatcherHelper
    {
        static Dispatcher _dispatcher;

        public static void Initialize()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void CheckBeginInvokeOnUi(Action action)
        {
            if (_dispatcher == null) throw new InvalidOperationException("The DispathcherHelper must be initialized before work!");

            _dispatcher.BeginInvoke(action);
        }
    }
}
