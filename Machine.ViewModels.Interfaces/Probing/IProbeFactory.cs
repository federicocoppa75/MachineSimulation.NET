using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Interfaces.Probing
{
    public interface IProbeFactory
    {
        IProbe Create(IProbableElement parent, Point point);
    }
}
