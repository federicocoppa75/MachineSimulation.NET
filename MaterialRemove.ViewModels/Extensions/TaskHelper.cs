using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels.Extensions
{
    internal static class TaskHelper
    {
        internal static Task<T> ToAsync<T>(Func<T> func)
        {
            return Task.Run(() => func());
        }

        internal static Task ToAsync(Action action)
        {
            return Task.Run(() => action);
        }
    }
}
