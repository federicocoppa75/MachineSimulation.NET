using Machine.Data.Base;
using Machine.ViewModels.Links;

namespace Machine.ViewModels.MachineElements
{
    public interface IViewTransformationFactory
    {
        object CreateTranslation(Matrix matrix, LinkViewModel link);
    }
}
