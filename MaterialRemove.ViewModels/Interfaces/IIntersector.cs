using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels.Interfaces
{
    public interface IIntersector
    {
        bool Intersect(IPanel panel);
        bool Intersect(IPanelSection section);
        bool Intersect(ISectionFace face);
    }
}
