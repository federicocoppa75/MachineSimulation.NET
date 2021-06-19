using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces;

namespace Machine.Views.ViewModels
{
    class StructViewModel : BaseViewModel
    {
        public IKernelViewModel Kernel { get; private set; }

        public StructViewModel() : base()
        {
            Kernel = Machine.ViewModels.Ioc.SimpleIoc<IKernelViewModel>.GetInstance();
        }
    }
}
