using MaterialRemove.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialRemove.ViewModels.Interfaces
{
    public interface ISectionPositionProvider
    {
        SectionPosition GetSectionPosition(int nxSection, int nySection, int i, int j);
    }
}
