using Machine.ViewModels;
using Machine.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

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
