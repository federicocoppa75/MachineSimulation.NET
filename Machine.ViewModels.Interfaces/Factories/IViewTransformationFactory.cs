using Machine.Data.Base;
using Machine.ViewModels.Interfaces.Links;

namespace Machine.ViewModels.Interfaces.Factories
{
    public interface IViewTransformationFactory
    {
        object CreateTranslation(Matrix matrix, ILinkViewModel link);
    }
}
