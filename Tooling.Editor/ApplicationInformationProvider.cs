using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooling.Editor
{
    internal class ApplicationInformationProvider : IApplicationInformationProvider
    {
        public ApplicationType ApplicationType => ApplicationType.ToolingEditor;
    }
}
