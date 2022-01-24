using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.UI
{
    public class ApplicationInformationProvider : IApplicationInformationProvider 
    {
        ApplicationType _applicationType;
        public ApplicationType ApplicationType => _applicationType;

        public ApplicationInformationProvider(ApplicationType applicationType)
        {
            _applicationType = applicationType;
        }
    }
}
