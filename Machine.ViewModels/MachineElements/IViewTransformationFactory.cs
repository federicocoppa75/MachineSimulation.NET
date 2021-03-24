using Machine.Data.MachineElements;
using Machine.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public interface IViewTransformationFactory
    {
        object CreateTranslation(Matrix matrix, LinkViewModel link);
    }
}
