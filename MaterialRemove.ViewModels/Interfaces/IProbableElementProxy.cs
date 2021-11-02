using System;
using System.Collections.Generic;
using System.Text;
using MVMIP = Machine.ViewModels.Interfaces.Probing;

namespace MaterialRemove.ViewModels.Interfaces
{
    public interface IProbableElementProxy : MVMIP.IProbableElement
    {
        void SetProbableElement(MVMIP.IProbableElement probableElement);
    }
}
