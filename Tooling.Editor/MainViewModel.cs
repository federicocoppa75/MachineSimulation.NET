using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMUI = Machine.ViewModels.UI;
using MVMUI = Machine.ViewModels.UI;
using MVM = Machine.ViewModels;

namespace Tooling.Editor
{
    internal class MainViewModel : Machine.ViewModels.MainViewModel
    {
        public VMUI.IOptionProvider DataSource => MVM.Ioc.SimpleIoc<VMUI.IOptionProvider>.GetInstance();
        public VMUI.IDataUnloader DataUnloader => MVM.Ioc.SimpleIoc<VMUI.IDataUnloader>.GetInstance();
        public VMUI.IToolingEditor ToolingEditor => MVM.Ioc.SimpleIoc<VMUI.IToolingEditor>.GetInstance();
    }
}
