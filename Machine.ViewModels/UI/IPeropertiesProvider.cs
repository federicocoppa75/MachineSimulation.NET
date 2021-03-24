using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public interface IPeropertiesProvider
    {
        ICollection<IFlag> Flags { get; }
        ICollection<IOptionProvider> Options { get; }
    }
}
